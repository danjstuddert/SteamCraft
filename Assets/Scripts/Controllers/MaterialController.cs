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
public enum ObjectType {Drone, BuzzSaw, GunBot, Boomer, ShieldWall, Factory, UpgradeStation}

// Controls the assignment of materials depending on the player that owns it
public class MaterialController : Singleton<MaterialController> {
	public TeamMaterials droneMaterials;
	public TeamMaterials buzzSawMaterials;
	public TeamMaterials gunBotMaterials;
	public TeamMaterials boomerMaterials;
	public TeamMaterials shieldWallMaterials;
	public TeamMaterials factoryMaterials;
	public TeamMaterials upgradeStationMaterials;

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
		case ObjectType.Drone:
			r.material = faction == Faction.Blue ? droneMaterials.blueMaterial : droneMaterials.redMaterial;
			break;
		case ObjectType.BuzzSaw:
			r.material = faction == Faction.Blue ? buzzSawMaterials.blueMaterial : buzzSawMaterials.redMaterial;
			break;
		case ObjectType.GunBot:
			r.material = faction == Faction.Blue ? gunBotMaterials.blueMaterial : gunBotMaterials.redMaterial;
			break;
		case ObjectType.Boomer:
			r.material = faction == Faction.Blue ? boomerMaterials.blueMaterial : boomerMaterials.redMaterial;
			break;
		case ObjectType.ShieldWall:
			r.material = faction == Faction.Blue ? shieldWallMaterials.blueMaterial : shieldWallMaterials.redMaterial;
			break;
		case ObjectType.Factory:
			r.material = faction == Faction.Blue ? factoryMaterials.blueMaterial : factoryMaterials.redMaterial;
			break;
		case ObjectType.UpgradeStation:
			r.material = faction == Faction.Blue ? upgradeStationMaterials.blueMaterial : upgradeStationMaterials.redMaterial;
			break;
		default:
			Debug.LogError (string.Format ("{0} is not a valid object cannot change its material", r.transform.name));
			break;	
		}
	}
}
