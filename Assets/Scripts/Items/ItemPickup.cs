using UnityEngine;

public class ItemPickup : Interactable
{
	public Item item;

	public override void Interact()
	{
		base.Interact();

		PickUp();
	}

	void PickUp()
	{
		Debug.Log("Pickup item: " + item.name);

		bool didPickUp = Inventory.instance.Add(item);
		if (didPickUp)
		{
			Destroy(gameObject);
		}
	}
}
