# Leo Adventure Cat

Jouez tout en aidant la cause animale !

Lâchement abandonné par son maître, aidez Léo à retrouver son frère à travers de nombreuses aventures. 

**La recette générée par ce jeu est reversée à une association animale** ([plus d'infos](#cause-animale)).

[More details on the game](https://maarti.net/project/LeoAdventureCat)


## Memo

### Remember
**Use adb logcat :**
Launch Command Prompt at adb.exe location (*C:\Users\[username]\AppData\Local\Android\android-sdk\platform-tools*) then type : 
```
adb logcat -s Unity ActivityManager PackageManager dalvikvm DEBUG
```

**Get device ID or AdMob :**
See [doc](https://developers.google.com/admob/android/test-ads)
Launch Command Prompt at adb.exe location (*C:\Users\[username]\AppData\Local\Android\android-sdk\platform-tools*) then type : 
```
adb shell
settings get secure android_id
```


**Tiled2Unity : Sorting Layer**
* [How to auto import Sorting Layer](http://www.seanba.com/megadadadventures.html)
* Create a string custom property named `sortingLayerName` on the Tiled layer, and set it with the name of the Unity sorting layer.
* For the collision layer, clic on the rectangle collider in the Tile Collision Editor and the `Type` property to match the physics layer in Unity.

[GooglePlayGames plugin](https://github.com/playgameservices/play-games-plugin-for-unity#google-play-games-plugin-for-unity)

### Graphismes
* **Bonus/Icônes :** Filtres > Distorsion de lentille
* Sprites : Contour avec crayon taille 2, puis selection, réduire de 1, inverser la sélection, adoucir de 2, supprimer
* LockedWorldImage : Filtre > Flou > Pixelisation > pixel size : 20
* Stars (world 2 background) : Filtres > Ombres et Lumières > Supernova

## Resources
* [How to animate a cat walking cycle](https://www.youtube.com/watch?v=dYCGMdQgs-I)
* [The Secret to Creating Perfect Color Palettes](https://gamedevelopment.tutsplus.com/articles/picking-a-color-palette-for-your-games-artwork--gamedev-1174) :
> **Hue** defines the tone of the actual color. For example, the color red has a hue value of 0 regardless of what you set the saturation and brightness values to. If you change that hue value to 120 you will have changed the color to green and if you change it to 240 the color will become blue.
> 
> Now what happens if we take one of those colors and give it a **saturation** value of 50? It looks as if you are picking a different color, but you are actually only affecting the intensity of the color - that is, how vivid the color is. Reducing the saturation makes the color look washed out.
> 
> **Brightness**, then, accounts for how light or dark the color is. If we reduced the brightness of a color, we would see this as a darker shade of that same color.
> 
> To create a great color palette you need only follow this rule:
> 
>     IF hues do not equal each other
>     THEN set saturations to match each other
>     AND set brightnesses to match each other
>     
>     ELSE IF saturations do not equal each other
>     THEN set hues to match each other
>     AND set brightnesses to match each other
>     
>     ELSE IF brightnesses do not equal each other
>     THEN set hues to equal each other
>     AND set saturations to equal each other
* [Free Country Flags](https://www.countryflags.com/en/)

## Author
[Bryan MARTINET](https://maarti.net)

## Credits
* Icons made by Smashicons from flaticon.com
* Most of music made by Kevin MacLeod (incompetech.com)
* UI (panel and buttons) made by Cameron Tatz (@CamTatz)