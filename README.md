[![Hardel](https://discord.com/assets/192cb9459cbc0f9e73e2591b700f1857.svg)](https://discord.gg/AP9axbXXNC)

# Too Many Roles Mods by Hardel

The "Too Many Roles Mod", is a mod for among Us, adding roles and perssonalization options.  
16 Game options are configurable in the lobby, They allow you to create the adjustments according to your needs.  
And currently has 3 roles : TimeMaster, Investigator and Lighter.
Several roles will be added in the future.

![Install](https://cdn.discordapp.com/attachments/790517195003527189/813239176412659752/Sans_titre.png)

# Releases :
| Among Us - Version| Mod Version | Link |
|----------|-------------|-----------------|
| 2020.12.19s | V1.2.1 | [Download](https://github.com/Hardel-DW/TooManyRolesMods/releases/download/V1.2.1/Among.Us.-.Too.Many.Roles.Modszip.zip) |
| 2020.12.19s | V1.2.0 | [Download](https://github.com/Hardel-DW/TooManyRolesMods/releases/download/V1.2/Among.Us.-.Too.Many.Roles.Mods.zip) |
| 2020.12.19s | V1.1.0 | [Download](https://github.com/Hardel-DW/TooManyRolesMods/releases/download/V1.1/Among.Us.-.ToManyRolesMods.zip) |
<details>
  <summary>Changelog</summary>
    <h2>Version 1.2.1</h2>
    <h3>Feature :</h3>
    <p>Added discord to PinkTracker.</p>
    <h3>Bug Correction :</h3>
    <p>  "Server didn't respond to modded handshake" was fixed</p>
</details>

# Installation
**Download the zip file on the right side of Github.**  
**Note: No private server is required, You can play on the official servers.**  
1. Find the folder of your game, for steams players you can right click in steam, on the game, a menu will appear proposing you to go to the folders.
2. Make a copy of your game, it's not obligatory but advise, put it where you want, I advise you to put it in the __commun__ folder of steam.
3. Drag or extract the files from the zip into your game, at the .exe level.
4. Turn on the game.
5. Play the game.

![Install](https://i.imgur.com/pvBAyZN.png)

# Roles Investigator :
 
### Description :
The investigator, is a role allowing to see the movements of the players.  
When a player moves, he leaves a footprint on the ground, it disappears after a while...  
The investigator can see this footprint.

### Note :
This role is given only to the Crewmate.  
One footprint cannot appear on another foorprint.

### Associated Game Options :
| Name | Description | Type |
|----------|:-------------:|------:|
| Enable Investigator | Allows you to activate the role. | Toggle |
| Number Investigator | Allows you to define the number of players that can have the role. | Number |
| Footprint Size | Sets the size of the footprint, The higher the value, the smaller the footprint.| Number |
| Footprint Interval | The interval is the duration of time between two footprints. | Number |
| Footprint Duration | Duration is the time the footprint remains on the ground. | Number |
| Anonymous Footprint | The color is the same for all players. | Toggle |
| Footprint are visible arround vent | If this option is disable the footprint are not visible around the vent. | Toggle |

-----------------------

# Roles TimeMaster :
### Description :
The Time Master has a button, by pressing it all the players go back in time gradually.  
If a player is dead during this time, he comes back to life.

### Note :
This role is available for crewmate and impostor.  
Reviving a player can be disabled.  
Tasks, Door, Sabotag and eOther button are not impacted.  
It is limited to 1 player maximum.

### Associated Game Options :
| Name | Description | Type |
|----------|:-------------:|------:|
| Enable Time Master | Allows you to activate the role. | Toggle |
| Enable Rivive during rewind | Allows you to disable the ability for the player to be revived. | Toggle |
| Rewind Duration | Defines the rewind time.| Number |
| Rewind Cooldown | It is the cooldown of the button.| Number |

---------------------------

# Roles Lighter :
### Description :
This is a role, having a different vision than a normal player, he can see during the light sabotage. if the option is activated 

### Note :
The light calculation is based on the crewmate's vision.  
For example: If the crewmates have a vision of 0.75, and Lighter has a multiplier of 2  
The Lighter will have a vision of 1.5.  
The multiplicative option can be less than 1.  

### Associated Game Options :
| Name | Description | Type |
|----------|:-------------:|------:|
| Enable Lighter | Allows you to activate the role. | Toggle |
| Number Lighter | Allows you to define the number of players that can have the role. | Number |
| Lighter Multiplier | Defines the multiplication of the vision in relation to the crewmate vision. | Number |
| Lighter sees during electrical sabotage | Allows you to see during the sabotage when the lights are off. | Toggle |

---------------

# Q&A
## Can you play Proximity Chat (Crewlink) with it?
Yes, Crewlink supports Among Us Modifications

## Can you get banned for playing on public Servers?
At the current state of the game there is no perma ban system for the game. The mod is designed in a way, that it does not send prohibited server requests. You are also able to join your own custom server to be safe [(Impostor)](https://github.com/Impostor/Impostor)

## Do my friends need to install the mod to play it together?
Yes. Every player in the lobby must have the mod installed.

If you'd like to contact me about anything else:
Discord: Hardel#7401
Discord Server: https://hardel.fr/discord

# Bugs/Feature suggestions
If you need to contact me, to request additional functionality, or make bug or change requests.  
Come on this discord server [Discord](https://discord.gg/s2TgC8Uj), or create a ticker on Github.

# Resources
https://github.com/NuclearPowered/Reactor The framework the mod uses.  
https://github.com/BepInEx For hooking game functions.  
https://github.com/DorCoMaNdO/Reactor-Essentials For creating custom game options easily.  
https://github.com/NotHunter101/ExtraRolesAmongUs For creating custom role.  
https://github.com/Woodi-dev/Among-Us-Sheriff-Mod For code snippets.  
https://github.com/tomozbot/AmongUsCustomRoles For code snippets.  
https://github.com/Aeolic/CultistMod For code snippets.  
https://github.com/Galster-dev/CrowdedSheriff For code snippters.  

# License
This software is distributed under the GNU GPLv3 License. BepinEx is distributed under LGPL-2.1 License.
