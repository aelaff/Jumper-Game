# Jumper-Game

# What I have made:
- Generating the ground grid size according to changeable variables.(width , height)
- Generating the player with it's name and random color and position.
- The main game play which controlling the ball through rigidbody using keyboard arrows for move and Space for jump.(note: I also create the movement script as you want but I comment it because the previous android problem,you can check playerControl script)
- When jumping , the related six ground pieces will be changed to the player color and the size of colored region will be changed according to the player scale (which depends on the score).
- When colliding through occupation process other player ,the other player will destroyed and the current player will gain its points.
- The game fully multiplayer functions through Firebase (I recently also used photons in other games), which instantiate the players , delete and modify them as game objects in the game though changing the Firebase values from the other and current players.
