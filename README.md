# Casino Wallet Console App

A .NET Core console application that simulates a player's wallet and slot game with a predefined probability model.

---

## Architecture

This project is built using **Clean Architecture** principles and **Domain-Driven Design (DDD)**. It promotes separation of concerns between the domain logic, application layer, infrastructure, and user interface.


---

## Development Environment

You can open and run the solution in:

- **Visual Studio 2022** (with .NET 8 SDK)
- **JetBrains Rider** (recommended for cross-platform users)
- Or any IDE that supports .NET 8 projects and solutions

---

### Running the Application

1. Make sure [.NET 8 SDK](https://dotnet.microsoft.com/download) is installed
2. In your terminal, run:

```bash
dotnet run --project CasinoWallet
```

The application will start, and you’ll be able to deposit, withdraw, place bets, and play the game.

Running Tests

If the solution contains a test project (CasinoWallet.Tests), run:
```bash
dotnet test
```

This will also execute the test GenerateWin_ProbabilityDistribution_MatchesSpecification, which outputs the RTP (Return To Player) estimate and win distribution. Use it to analyze game fairness and balance.

⸻

Features
•	Initial player balance is $0
•	Deposit and withdrawal of funds
•	A slot-style game with simulated win probabilities:
•	Bet range: $1 to $10
•	Win probabilities:
•	50% — loss (x0)
•	40% — small win (x1.0–x2.0)
•	10% — big win (x2.0–x10.0)
•	Balance update after each bet:
•	The game continues until the player decides to exit

⸻

RTP Test Example Output

Output from the GenerateWin_ProbabilityDistribution_MatchesSpecification test:
•	Trials: 1,000,000
•	Lose (x0): 500,023 (50.00%)
•	Small Win (x1.0–x2.0): 399,533 (39.95%)
•	Big Win (x2.0–x10.0): 100,444 (10.04%)
•	Small Win Multiplier Range: [1.00, 2.00]
•	Big Win Multiplier Range: [2.00, 10.00]
•	Estimated RTP: 120.08%

⸻

Important
•	All amounts (deposit, withdrawal, bet) must be positive numbers
•	The application assumes valid user input

⸻