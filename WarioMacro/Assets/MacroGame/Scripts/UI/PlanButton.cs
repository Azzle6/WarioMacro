using UnityEngine;
using UnityEngine.EventSystems;

public class PlanButton : MonoBehaviour
{
	public bool locked;
	[SerializeField] private Animator animator;
	private bool isSelected, wasSelected;
	private static readonly int isHovered = Animator.StringToHash("isHovered");

	private void Update()
	{
		if (locked)
		{
			return;
		}
		isSelected = (EventSystem.current.currentSelectedGameObject == gameObject);

		if (isSelected != wasSelected)
		{
			wasSelected = isSelected;
			if (isSelected)
			{
				animator.SetBool(isHovered, true);
			}
			else
			{
				animator.SetBool(isHovered, false);
			}
		}
	}
}
