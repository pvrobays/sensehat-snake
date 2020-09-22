# Snake for the Raspberry Pi's Sense Hat
Simple snake game to play via the joystick of the SenseHat for the Raspberry Pi.

# Installation
1. Install the .net core runtime on your Raspberry Pi (https://edi.wang/post/2019/9/29/setup-net-core-30-runtime-and-sdk-on-raspberry-pi-4)
2. Get the source code from this project
3. Build and publish the project as `framework dependent`, `portable` binaries `dotnet publish --self-contained false`
4. Copy the files to your Raspberry pi, and `cd` to them.
5. Run it! `dotnet SenseHat.Snake.dll` 


# How to play?
- Use the `up` / `down`/ `left` / `right` joy stick to change the direction of the snake
- Try to catch as many red apples as possible
- Don't eat yourself!
- There are no walls, you just teleport to the other side by some dark magic
- Quit the game by `click`-ing on the joystick
- The final score will be shown in the console