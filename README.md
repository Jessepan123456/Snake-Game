# Snake Game (Multiplayer)
A real-time multiplayer Snake game built using a client-server approach. Players control snakes, compete in a game space, and try to survive as long as possible while collecting food.

## Features
- Multiplayer gameplay with multiple connected clients
- Real-time synchronization between players
- Snake movement
- Game state updates
- Web Server that displays a leaderboard

## Networking
- Client-server model for game state
- Clients handle:
  - Player input
  - Rendering the game state

## Additional Features
- Randomize the custom skin between each snake
- Custom background and Custom map

## How to Run
1. Clone the repository
2. Run the server provided by the folder first
3. Then run your client

## What I Learned
- How to design a client-server for real-time games
- Pair programming
- Managing shared state across multiple clients, preventing race conditions
- Structuring Code using MVC Pattern

## Future Improvement
- Add more power-ups or obstacles
- Better UI
- A Growth system for the snakes
