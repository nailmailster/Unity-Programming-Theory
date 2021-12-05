using UnityEngine;

//  INHERITANCE
public class CowController : AnimalController
{
    [SerializeField] public CowFSM cowFSM;
    [SerializeField] public Sight sight;

    AudioSource moo;

    public CowController()
    {
        scale = 0.07f;
    }

    //  POLYMORPHISM
    protected override void Awake()
    {
        base.Awake();
        moo = GetComponent<AudioSource>();
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
            animator.SetFloat("Speed_f", .5f);
            rb.MovePosition(rb.position + transform.forward * Time.deltaTime);
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
            agent.isStopped = false;
            agent.enabled = true;
            cowFSM.CurrentState = CowState.Stay;
            cowFSM.enabled = true;
            sight.enabled = true;

            IsActive = false;
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
                cow.GetComponent<CowController>().agent.isStopped = false;
                cow.GetComponent<CowController>().agent.enabled = true;

                cow.GetComponent<CowController>().cowFSM.CurrentState = CowState.Stay;
                cow.GetComponent<CowController>().cowFSM.enabled = true;

                cow.GetComponent<CowController>().sight.enabled = true;
            }
        }

        GameObject dog = GameObject.FindGameObjectWithTag("Dog");
        dog.GetComponent<DogController>().IsActive = false;
        dog.GetComponent<DogController>().selectionRing.SetActive(false);
        dog.GetComponent<DogController>().agent.enabled = true;
        dog.GetComponent<DogController>().agent.isStopped = false;

        IsActive = true;
        selectionRing.SetActive(true);

        agent.isStopped = true;
        agent.enabled = false;
        cowFSM.CurrentState = CowState.Walk;
        cowFSM.StopAllCoroutines();
        cowFSM.enabled = false;
        sight.enabled = false;
        animator.SetBool("Eat_b", false);

        moo.Play();
    }

    private void OnEnable()
    {
        EventManager.StartListening("GroundOnMouseDown", Moo);
    }

    private void OnDisable()
    {
        if (EventManager.instance != null)
            EventManager.StopListening("GroundOnMouseDown", Moo);
    }

    void Moo(Vector3 destination)
    {
        if (IsActive)
        {
            moo.Play();

            selectionRing.SetActive(false);
            agent.enabled = true;
            agent.isStopped = false;
            cowFSM.CurrentState = CowState.ChaseDestination;
            cowFSM.enabled = true;
            sight.enabled = true;

            IsActive = false;

            cowFSM.SendMessage("ChaseDestination", destination);
        }
    }
}
