using System.Collections;
using System.Linq;
using UnityEngine;

public class Map : MonoBehaviour
{ 
    public Node currentNode { get; private set; }
    
    [SerializeField] private Player player;
    [SerializeField] private Node startNode;
    [SerializeField] private Node endNode;
    
    private static readonly int current = Animator.StringToHash("Current");


    public void Load()
    {
        gameObject.SetActive(true);
        currentNode = startNode;
        player.TeleportPlayer(currentNode.transform.position);
    }

    public void Unload()
    {
        gameObject.SetActive(false);
    }

    public bool OnLastNode()
    {
        return currentNode == endNode;
    }
    
    public IEnumerator WaitForNodeSelection()
    {
        // init
        var arrowPrefabs = player.arrowPrefabs.ToList();
        var nextNode = default(Node);
        var selectedNode = default(Node);
        var selectedDirection = -1;
        // ReSharper disable once TooWideLocalVariableScope
        bool selectInput;
        var lastDirectionSelected = -1;
        const ControllerKey validInput = ControllerKey.A;

        arrowPrefabs.ForEach(go => go.SetActive(true));
        
        currentNode.animator.SetBool(current, true);
        
        // input loop
        while (nextNode == null)
        {
            // ReSharper disable once PossibleNullReferenceException
            foreach (Node.Path path in currentNode.paths)
            {
                var hAxis = Input.GetAxis("LEFT_STICK_HORIZONTAL");
                var vAxis = Input.GetAxis("LEFT_STICK_VERTICAL");
                var isUp = vAxis > 0.5f;
                var isDown = vAxis < -0.5f;
                var isRight = hAxis > 0.5f;
                var isLeft = hAxis < -0.5f;
                
                
                selectInput = (path.direction == Node.Direction.Up && isUp)
                              || (path.direction == Node.Direction.Down && isDown)
                              || (path.direction == Node.Direction.Left && isLeft)
                              || (path.direction == Node.Direction.Right && isRight);

                if (!selectInput) continue;
                
                selectedNode = path.destination;
                selectedDirection = (int)path.direction;
                if (selectedDirection != lastDirectionSelected) AudioManager.MacroPlaySound("MOU_NodeDirection", 0);
                lastDirectionSelected = selectedDirection;
                break;
            }

            for (int i = 0; i < arrowPrefabs.Count; i++)
            {
                // is any path setup with arrow direction?
                arrowPrefabs[i].gameObject.SetActive(currentNode.paths.FirstOrDefault(p => p.direction == (Node.Direction)i) != null);
                // is the selected direction equals to the path direction?
                arrowPrefabs[i].transform.localScale = i == selectedDirection ? Vector3.one : Vector3.one * .5f;
            }
            
            if (selectedNode != null && InputManager.GetKeyDown(validInput))
            {
                nextNode = selectedNode;
            }
            yield return null;
        }
        
        currentNode.animator.SetBool(current, false);
        currentNode = nextNode;

        // dispose
        arrowPrefabs.ForEach(go => go.SetActive(false));
    }
    
    private void Awake()
    {
        currentNode = startNode;
        
    }
}
