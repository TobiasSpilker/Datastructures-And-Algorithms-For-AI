# 📚 Algorithms & Data Structures — C# Assignments

This repository bundles **12** independent console programs written during the *Data Structures & Algorithms* course at Utrecht University.  
Each file is self-contained; there are **no project-level dependencies** between the assignments.

| File | Topic | One-liner |
| ---- | ----- | --------- |
| **`AirTraffic.cs`** | Computational geometry | Divide-and-conquer closest-pair algorithm that computes the minimum squared distance between aircraft positions. |
| **`Annagram.cs`** | Graph/BFS | Bit-packed breadth-first search over word permutations; supports three output modes (*L*, *S*, *A*). |
| **`Saw.cs`** | Greedy + sorting | Finds the minimum cost to cut an *H × B* wooden board into unit squares (board-cutting problem). |
| **`Spy.cs`** | Linked-list cursor | Reconstructs passwords typed with cursor moves `<`, `>` and backspace `-` using a custom doubly-linked list. |
| **`Delivery.cs`** | Binary search | Determines the smallest van capacity *B* that ships all packages in ≤ *R* trips. |
| **`Discs.cs`** | Binary search | For each customer diameter, picks the nearest smaller catalog block and counts extra “chips” needed. |
| **`Fractions.cs`** | Merge sort | Reads fractions and outputs three sorted orders: by numerator, denominator, and numeric value. |
| **`LinkedLists.cs`** | Mutable list | Implements a doubly-linked list with add/remove mutations and reporting modes (**C**ount, **S**erialise, **D**istribution, **P**rint ops). |
| **`Collatz.cs`** | Math loop | Prints the Collatz sequence length for every input integer. |
| **`PlanetX.cs`** | I/O warm-up | Sums pairs of integers — simple practice with reading and writing data. |
| **`Silhouet.cs`** | Skyline (partial) | Sketch of a divide-and-conquer skyline merger for rectangular buildings (work in progress). |
| **`Store.cs`** | Skeleton | Input-parsing stub for a yet-to-be-implemented warehouse simulation. |

---

## 🛠 How to build & run

All sources target **.NET 6+** and compile as single-file programs.  
To try one:

```bash
# example: run AirTraffic.cs
dotnet new console -n tmp
cp AirTraffic.cs tmp/Program.cs
cd tmp
dotnet run < input.txt
```

Each file contains inline comments and, where relevant, sample I/O blocks.

---

*Feel free to cherry-pick only the modules you need. Happy coding!*
