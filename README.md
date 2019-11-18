# Red-Back-Tree
## Put
`Tree<int> tree = new Tree<int> (new int[] { 9, 7, 11, 4, 8, 10, 13, 3, 5, 12, 15, 14, 16 });`
## Show Data
`Console.Write (tree.ToString ());`
```
9
├── 7
│   ├── 4
│   │   ├── 3
│   │   └── 5
│   └── 8
└── 11
    ├── 10
    └── 13
        ├── 12
        └── 15
            ├── 14
            └── 16
```
## Show Color
`Console.Write (tree.ToString (true));`
```
BACK
├── BACK
│   ├── BACK
│   │   ├── RED
│   │   └── RED
│   └── BACK
└── BACK
    ├── BACK
    └── RED
        ├── BACK
        └── BACK
            ├── RED
            └── RED
```
