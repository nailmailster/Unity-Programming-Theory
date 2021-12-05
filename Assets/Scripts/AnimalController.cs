using UnityEngine;

using UnityEngine.AI;

//  POLYMORPHISM
public class AnimalController : MonoBehaviour
{
    protected float scale;

    protected Rigidbody rb;
    protected Animator animator;

    public GameObject selectionRing;
    [SerializeField] protected Texture2D handCursorTexture;
    [SerializeField] protected Texture2D arrowCursorTexture;

    //  ENCAPSULATION
    private bool isActive;
    public bool IsActive
    {
        get
        {
            return isActive;
        }
        set
        {
            isActive = value;
        }
    }

    public NavMeshAgent agent;

    //  INHERITANCE
    public AnimalController()
    {
        IsActive = false;
    }

    //  POLYMORPHISM
    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    //  INHERITANCE
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        animator.SetFloat("Speed_f", 0);
    }

    //  INHERITANCE
    void Update()
    {
        //  ABSTRACTION
        InputProcessing();
    }

    //  protected - ENCAPSULATION, virtual - POLYMORPHISM
    protected virtual void InputProcessing()
    {
    }

    //  INHERITANCE
    private void FixedUpdate()
    {
        CorrectZ();
    }

    //  INHERITANCE
    void CorrectZ()
    {
        Vector3 eulerAngles = transform.eulerAngles;
        transform.eulerAngles = new Vector3(eulerAngles.x, eulerAngles.y, 0);
    }
}
