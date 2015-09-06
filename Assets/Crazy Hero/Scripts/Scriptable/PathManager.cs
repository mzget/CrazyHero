using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct PathManager {

	public const string PATH_OF_GUI_PREFABS = "GUI_objects/";
	public const string PATH_OF_AUDIO_OBJECTS = "Audio_Objects/";
    public const string Path_of_audioclips = "AudioClips/";
	public const string PATH_OF_HUD_OBJECTS = "Prototypes/HUD/";
	public const string PATH_OF_GUI_OBJECTS = "Prototypes/GUI/";
    public const string PATH_OF_STAGES_OBJECTS = "Stages/";
	public const string PATH_OF_CARDS = "Prototypes/Cards/";
	public const string PATH_OF_UNIT_PREFABS = "Prototypes/Units/";
	public const string PATH_OF_HEROES_PREFABS = "Prototypes/Characters/";
    public const string Path_of_monsters = "Prototypes/Monsters/";
	public const string Path_Of_Effects = "Effects/";

	public static Dictionary<int,string> PATH_MONSTERS_DICT = new Dictionary<int,string>() {
		{ 1 , "Prototypes/Monsters/Doom"},
		{ 2 , "Prototypes/Monsters/Spider"},
		{ 3 , "Prototypes/Monsters/Monster01"},
		{ 4 , "Prototypes/Monsters/Sand"},
		{ 5 , "Prototypes/Monsters/Dog Warrior"},
		{ 6 , "Prototypes/Monsters/Dog Archer"},
		{ 7 , "Prototypes/Monsters/Dog Mage"},
		{ 8 , "Prototypes/Monsters/Jack O Lantern"},
		{ 9 , "Prototypes/Monsters/Destroyer wheel"},
		{ 10 , "Prototypes/Monsters/Mistress"},
		{ 11 , "Prototypes/Monsters/Savage boar"},		
		{ 12 , "Prototypes/Monsters/BrownCyclop"},
		{ 13 , "Prototypes/Monsters/Dark Cyclop"},
		{ 14 , "Prototypes/Monsters/Slime"},
	};
};

public struct TagManager {
	public const string GUItag = "GUI";
	public const string Boss = "Boss";
	public const string MONSTER = "Monster";
	public const string HERO = "Hero";
	public const string UNIT = "Unit";
};