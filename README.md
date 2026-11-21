# 🎰 Slots — 2D Slot Machine

A classic slot machine built in Unity using C#. The project focuses on recreating the basic slot machine experience, including spinning reels, randomized outcomes, payouts, and smooth reel animations.

## 🎮 Play the Game

**[Play the WebGL Build on itch.io](https://sparkyyyboiii.itch.io/slots)**

The game is playable directly in the browser. No installation is required.

## 🎯 Game Overview

The objective is simple: place a bet, pull the lever, and try to get three matching symbols.

The machine has three spinning reels, each displaying symbols such as Cherry, Bell, Bar, and Seven. Every spin generates a result, and the payout depends on the matching combination.

- Starting balance: $5,000
- Minimum bet: $10
- Three spinning reels
- Four different symbols
- Different payouts depending on the winning symbol

### Payouts

| Winning combination | Payout |
|---|---:|
| 🍒 Cherry × 3 | 1.5× bet |
| 🔔 Bell × 3 | 1.75× bet |
| Bar × 3 | 2× bet |
| 7️⃣ Seven × 3 | 3× bet |

## 🕹️ How to Play

1. Open the game using the itch.io link above.
2. Enter your bet amount.
3. Click the lever to start the spin.
4. Wait for all three reels to stop.
5. Match all three symbols to win a payout.

Your bet is deducted when the spin begins. If you win, the corresponding payout is added to your balance. Otherwise, you lose the amount you wagered.

## 🛠️ Technologies Used

- **Unity** – Game engine
- **C#** – Gameplay logic and scripting
- **DOTween** – Reel animation and movement
- **Unity Sprite Renderer & Sprite Mask** – Rendering and clipping the reel symbols

## ⚙️ Implementation & Thought Process

I wanted to build the slot machine in a way where the game logic and visual animations were handled separately. The outcome of a spin should be decided before the reels start moving, rather than having the animation determine the result.

Here are some of the implementation decisions I made along the way.

### 1. Random Number Generation

Instead of directly relying on Unity's random number generation for every outcome, I implemented a Linear Congruential Generator (LCG).

The generator follows:

`X(n+1) = (a × X(n) + c) mod m`

I used a symbol pool containing 100 entries with the following distribution:

- Cherry: 40 entries
- Bell: 30 entries
- Bar: 20 entries
- Seven: 10 entries

The generator selects indices from this pool to produce the three reel results. This gives each symbol a different probability of appearing.

### 2. Reel Animation

Each reel contains multiple copies of the available symbols arranged vertically.

Rather than moving the entire reel and resetting its position, I implemented a recycling mechanism. As symbols move past the bottom boundary, they are repositioned above the other symbols. This creates continuous reel movement.

Sprite Masks are used to keep the symbols visible only within their respective reel windows.

### 3. Acceleration and Deceleration

One of the more interesting parts of the project was getting the reels to stop naturally.

Initially, I used separate spinning and settling animations. However, this caused noticeable jumps when the reel reached its final result.

I eventually changed the approach to use distance-based deceleration.

The reel accelerates, maintains its maximum speed for a specified duration, and then decelerates towards its predetermined target symbol. The deceleration is calculated using:

`v² = u² + 2as`

This allows the reel's stopping distance to depend on its current speed and deceleration rate, rather than forcing it to stop within an arbitrary animation duration.

The goal was to make the final result feel like a natural consequence of the reel's movement instead of an animation snapping to a predetermined symbol.

### 4. Game Logic and Payout System

I separated the responsibilities of the game into different scripts:

- **GameManager:** Handles the overall game flow, bets, cash balance, and spin completion.
- **RNGManager:** Generates the results for each spin.
- **ReelController:** Handles the movement and stopping behaviour of individual reels.
- **ReelSymbol:** Stores the symbol type associated with each symbol object.
- **PayoutManager:** Calculates winnings based on the final combination.
- **HUDController:** Handles the user interface and balance display.

The GameManager waits until all three reels have finished before resolving the result and updating the player's balance.

## ✨ Bonus Features

- Staggered reel stopping, where each reel can have a different spinning duration.
- Weighted symbol distribution.
- Separate reel controllers for independent movement.
- Bet validation to prevent invalid bets.
- Automatic payout calculation.
- Continuous symbol recycling for seamless reel movement.

## 📁 Project Structure

```text
Assets/
├── Animations/
├── Prefabs/
├── Scripts/
│   ├── GameManager.cs
│   ├── RNGManager.cs
│   ├── ReelController.cs
│   ├── ReelSymbol.cs
│   ├── PayoutManager.cs
│   └── ...
├── UI/
├── Sounds/
└── ...
```

*The structure above represents the intended organization of the project. Some folders may be omitted if they are not applicable. Like for example the build folder is ommitted since the game was already published and playable online at Itch*

## 🚧 What I Learned

Some of the key things I worked on were:

- Implementing a custom random number generator.
- Working with probability and weighted outcomes.
- Managing multiple independently animated game objects.
- Using DOTween for animation and interpolation.
- Understanding velocity, acceleration, and stopping distance.
- Separating game state, visual behaviour, and payout calculations.
- Handling asynchronous completion of multiple reel animations.

The most challenging part was getting the reels to stop smoothly on the correct symbols without visible snapping or sudden corrections.

---

**Built with Unity and C#**
