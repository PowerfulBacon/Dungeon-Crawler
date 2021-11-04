# Dungeon Crawler
 A multiplayer dungeon crawler with voxel based graphics

## Unity Version
2021.1.15f1

## Getting Started
Download the project and open in Unity. You may need to install Unity Version 2021.1.15f1.
You will need to go to Window > Photon Unity Networking and run the setup wizard, providing a project ID for a create PUN application. (You will need to create this for testing)

## Contribution Guide
All contributions are welcome, contribute anything you want. Stuff on the projects page will be accepted always if the code is good, stuff on there may be accepted or rejected depending on how good the idea is.
Not only code is required, creating issues and organising the project is helpful, creating new models for the game, textures or sounds is great too. Everything is welcome.

### Modelling
For modelling, I use MagicaVoxel. To get the correct model sizes when importing I recommend going to the config.txt of MagicaVoxel and replacing io_obj with
```
io_obj :
{
	scale			: '0.03125'
	axis			: 'XZ-Y'	// axis : { 'XYZ' : Z up; 'XZ-Y' Y up }
	cw			: '0'		// { 0 : counter clockwise; 1 : clockwise }

	merge			: '1'		// merge voxel faces with same color
}
```
as this is the config currently being used on the repository to generate .obj files from models.

### Licence
I haven't decided the licence yet so for legal reasons or whatever I own all the copyright for the code here and have the right to change it to whatever licence that gets decided on being used.
