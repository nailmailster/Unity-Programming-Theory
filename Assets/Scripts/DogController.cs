using System.Collections;
using UnityEngine;

//  INHERITANCE
public class DogController : AnimalController
{
    [SerializeField] public DogFSM dogFSM;
    [SerializeField] public Sight sight;

    [SerializeField] AudioSource woof;
    [SerializeField] AudioSource theme;

    public DogController()
    {
        scale = 6f;
    }

    //  POLYMORPHISM
    protected override void Awake()
    {
        base.Awake();
    }

    //  protected - ENCAPSULATION, virtual - POLYMORPHISM
    protected override void InputProcessing()
    {
        if (!IsActive)
            return;
        
        if (Input.GetKey(KeyCode.X))
        {
            transform.Rotate(0, -45 * Time.deltaTime, 0);
        }
        if (Input.GetKey(KeyCode.C))
        {
            transform.Rotate(0, 45 * Time.deltaTime, 0);
        }

        if (Input.GetKey(KeyCode.Z))
        {
            animator.SetFloat("Speed_f", 1);
            rb.MovePosition(rb.position + transform.forward * Time.deltaTime * 2.5f);
        }
        else
        {
            animator.SetFloat("Speed_f", 0);
        }
        if (Input.GetKey(KeyCode.V))
        {
            animator.SetFloat("Speed_f", .5f);
            rb.MovePosition(rb.position - transform.forward * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            selectionRing.SetActive(false);
            agent.enabled = true;
            agent.isStopped = false;
        }
    }

    private void OnMouseEnter()
    {
        Cursor.SetCursor(handCursorTexture, Vector2.zero, CursorMode.Auto);
    }

    private void OnMouseExit()
    {
        Cursor.SetCursor(arrowCursorTexture, Vector2.zero, CursorMode.Auto);
    }

    private void OnMouseDown()
    {
        GameObject[] cows = GameObject.FindGameObjectsWithTag("WhiteCow");
        foreach (GameObject cow in cows)
        {
            cow.GetComponent<CowController>().IsActive = false;
            cow.GetComponent<CowController>().selectionRing.SetActive(false);

            if (!cow.GetComponent<CowController>().agent.enabled)
            {
                cow.GetComponent<CowController>().agent.enabled = true;
                cow.GetComponent<CowController>().agent.isStopped = false;

                cow.GetComponent<CowController>().cowFSM.CurrentState = CowState.Stay;
                cow.GetComponent<CowController>().cowFSM.enabled = true;

                cow.GetComponent<CowController>().sight.enabled = true;
            }
        }

        IsActive = true;
        selectionRing.SetActive(true);

        animator.SetBool("Sit_b", false);

        agent.isStopped = true;
        agent.enabled = false;
        dogFSM.CurrentState = DogState.Walk;
        dogFSM.StopAllCoroutines();
        dogFSM.enabled = false;
        sight.enabled = false;
        StartCoroutine(BarkTwice());
    }

    IEnumerator BarkTwice()
    {
        animator.enabled = false;
        animator.SetBool("Bark_b", true);
        animator.enabled = true;

        woof.Play();
        yield return new WaitForSeconds(.5f);
        woof.Play();
        yield return new WaitForSeconds(.5f);
        woof.Play();

        animator.SetBool("Bark_b", false);
    }

    private void OnEnable()
    {
        EventManager.StartListening("GroundOnMouseDown", Woof);
    }

    private void OnDisable()
    {
        if (EventManager.instance != null)
            EventManager.StopListening("GroundOnMouseDown", Woof);
    }

    void Woof(Vector3 destination)
    {
        if (IsActive)
        {
            woof.Play();
            theme.Play();

            selectionRing.SetActive(false);
            if (agent.isActiveAndEnabled)
                agent.isStopped = false;
            agent.enabled = true;
            dogFSM.CurrentState = DogState.ChaseDestination;
            dogFSM.enabled = true;
            sight.enabled = true;

            IsActive = false;

            dogFSM.SendMessage("ChaseDestination", destination);
        }
    }
}
