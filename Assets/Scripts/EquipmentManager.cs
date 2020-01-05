using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
	public static EquipmentManager instance;
	void Awake()
	{
		if (instance != null)
		{
			Debug.LogWarning("More than one instance of EquipmentManager found!");
			return;
		}

		instance = this;
	}

	public Equipment[] defaultItems;
	public SkinnedMeshRenderer targetMesh;
	Equipment[] currentEquipment;
	SkinnedMeshRenderer[] currentMeshes;

	public delegate void OnEquipmentChanged(Equipment newItem, Equipment oldItem);
	public OnEquipmentChanged onEquipmentChanged;

	Inventory inventory;

	void Start()
	{
		inventory = Inventory.instance;

		int numSlots = System.Enum.GetNames(typeof(EquipmentSlot)).Length;
		currentEquipment = new Equipment[numSlots];
		currentMeshes = new SkinnedMeshRenderer[numSlots];

		EquipDefaultItems();
	}

	public void Equip(Equipment newItem)
	{
		int slotIndex = (int)newItem.equipmentSlot;

		Equipment oldItem = Unequip(slotIndex);
		currentEquipment[slotIndex] = newItem;

		SetEquipmentBlendShapes(newItem, 100);

		SkinnedMeshRenderer newMesh = Instantiate<SkinnedMeshRenderer>(newItem.mesh);
		newMesh.transform.parent = targetMesh.transform;

		newMesh.bones = targetMesh.bones;
		newMesh.rootBone = targetMesh.rootBone;
		currentMeshes[slotIndex] = newMesh;

		if (onEquipmentChanged != null)
		{
			onEquipmentChanged.Invoke(newItem, oldItem);
		}
	}

	public Equipment Unequip(int slotIndex)
	{
		Equipment oldItem = currentEquipment[slotIndex];
		if (oldItem != null)
		{
			if (currentMeshes[slotIndex] != null)
			{
				Destroy(currentMeshes[slotIndex].gameObject);
			}
			SetEquipmentBlendShapes(oldItem, 0);

			inventory.Add(oldItem);
			currentEquipment[slotIndex] = null;

			if (onEquipmentChanged != null)
			{
				onEquipmentChanged.Invoke(null, oldItem);
			}
		}
		return oldItem;
	}

	public void UnequipAll()
	{
		for (int i = 0; i < currentEquipment.Length; i++)
		{
			Unequip(i);
		}

		EquipDefaultItems();
	}

	void SetEquipmentBlendShapes(Equipment item, int weight)
	{
		Debug.Log("SetEquipmentBlendShapes for: " + item.name + "; Weight: " + weight);
		foreach (EquipmentMeshRegion blendShape in item.coveredMeshRegions)
		{
			Debug.Log("Set Blend Shape: " + blendShape);
			targetMesh.SetBlendShapeWeight((int)blendShape, weight);
		}
	}

	void EquipDefaultItems()
	{
		foreach (Equipment item in defaultItems)
		{
			Equip(item);
		}
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.U))
		{
			UnequipAll();
		}
	}
}
