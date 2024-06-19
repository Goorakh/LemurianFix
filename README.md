# Lemurian Fix

Fixes the game often not being able to tell the difference between Devoted Lemurians and regular Lemurians.

This fixes:
* Regular Lemurian spawn sound.
* Lemurian ghosts summoned by killing a lemurian with Happy Mask having Devoted AI and persisting to the next stage.
* And probably more!

## Technical details

This mod assigns Devoted Lemurians their own body prefab ("DevotedLemurianBody") instead of sharing one with regular Lemurians, and also removes the Devoted body flag from regular Lemurians.