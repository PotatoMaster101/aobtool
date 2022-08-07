# AOB Tool
Small tool to help working with AOB when using [Cheat Engine](https://cheatengine.org/).

## Usage
### Count AOB Length
Counts the byte array length.
```
$ aobtool count <aob>
```

Example:
```
$ aobtool count 7458FCFF03
5
```

### Format AOB
Formats the byte array.
```
$ aobtool format <aob>
```

Example:
```
$ aobtool format 7458FCFF03
74 58 FC FF 03
```

### AOB Diff
Performs diff between multiple AOBs in a file, 1 AOB per line.
```
$ aobtool diff <aob file> [--wildcard <wildcard character>]
```

Example:
```
$ cat aob.txt
75 49 CC FE 54 98 53 69 44
75 49 CC FE 54 98 53 69 FF
75 49 CC 0E 54 97 53 69 44
75 49 CC FE 54 98 53 69 44 E0
$ aobtool diff aob.txt
75 49 CC ?E 54 9? 53 69 ?? ??
```

## Build
Build using [`dotnet`](https://dotnet.microsoft.com/download):
```
$ dotnet build -c Release
```

## Test
Run unit tests using [`dotnet`](https://dotnet.microsoft.com/download):
```
$ dotnet test
```
