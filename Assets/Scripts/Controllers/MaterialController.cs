using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
// A struct that is used to set materials for each player
public struct TeamMaterials{
	public Material blueMaterial;
	public Material redMaterial;
}

// An identifier to set what type of object that this is
public enum ObjectType {Bot, Factory, UpgradeStation}

// Controls the assignment of materials depending on the player that owns it
public class MaterialController : Singleton<MaterialController> {
// botMaterials is a set of materials used for bots
	public TeamMaterials botMaterials;
// factoryMaterials is a set of materials used for bots
	public TeamMaterials factoryMaterials;
// upgradeStationMaterials is a set of materials used for bots
	public TeamMaterials upgradeStationMaterials;
// upgradeStationHoloMaterials is a set of materials used for bots
	public TeamMaterials hologramMaterials;

//----------------------------------------------------------
//	UpdateMaterial()
// Updates the material of the given transform to the correct players
// colour
//
// Param:
//		Transform t - the transform to change
//		ObjectType type - the type of object that it is
//		Faction faction - the faction that owns the transform
// Return:
//		Void
//----------------------------------------------------------
	public void UpdateMaterial(Renderer r, ObjectType type, Faction faction){
		switch(type){
		case ObjectType.Bot:
				r.material = faction == Faction.Blue ? botMaterials.blueMaterial : botMaterials.redMaterial;
				break;
		case ObjectType.Factory:
			r.material = faction == Faction.Blue ? factoryMaterials.blueMaterial : factoryMaterials.redMaterial;
			break;
		case ObjectType.UpgradeStation:
			r.material = faction == Faction.Blue ? upgradeStationMaterials.blueMaterial : upgradeStationMaterials.redMaterial;
			r.transform.GetComponentInChildren<Renderer>().material = faction == Faction.Blue ? hologramMaterials.blueMaterial : hologramMaterials.redMaterial;
				break;
		default:
			Debug.LogError (string.Format ("{0} is not a valid object cannot change its material", r.transform.name));
			break;	
		}
	}
}
