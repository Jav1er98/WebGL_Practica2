## Second activity WebGL.
After making the modifications to the base project with a controller that allows you to upload a set of meshes and establish a Pawn, give the Pawn movement. The controller shall detect the keystrokes for the forward and reverse command. The direction of movement is obtained from the orientation of the camera.

To carry out this activity, what we must do is modify the file “Controller.razor.cs” and “Game.razor.cs” by changing a few lines of code corresponding to the functions keydownEvent and keyupEvent that detect the keyboard keys. In my case I have also assigned the movement of left and right for the pawn with the keys "a" and "d".

![gif ejercicio 1](/images/Captura1.png)
![gif ejercicio 1](/images/Captura2.png)

In the methods "keydownEvent()" and "keyupEvent()", when we press the key, we make a calculation with the yaw angle to know which direction to move within the XZ plane and the second one is called when we stop pressing the key and sets to 0 all the impulses in XYZ.

Inside "Controller.razor.cs" we have an interface with a method (GetMovement()) that returns a vector indicating the movement to a direction.

In the method "updatePawn()", that we can locate in “Game.razor.cs” we get the vector of the Controller, which tells us the direction to which the Pawn wants to move, if the normal is greater than 0, we proceed to move the pawn and rotate it in that direction.

![gif ejercicio 1](/images/Captura4.png)

Finally we need to make the call to these methods.

![gif ejercicio 1](/images/Captura3.png)

The result is:

![gif ejercicio 1](/images/gif.gif)
