# Sugar.lang
Sugar is a programming language I created a while back. Its object oriented, written in c#, (will) compiles to CIL.

## Features

### Features that have been implemented: 
`Variables`, `Primitives`, `Arrays`, `Functions`, `Output and Input`, `Generics`, `Constructors`, `Indexers`, `Ternary`, `Casting`, `Conditions (if, switch)`, `Loops (for, while, do while)`, `User Defined Data Types`, `Properties`, `Exceptions`, `Import`, `Describers`,
`Operator and Cast Overloading`

*Note: At the current stage "implemented" means they are parsed successfully.*
### Planned Features 
`Extension Methods`

## Basic Syntax

### • Variables
```cs
//general format for variable creation
Type: name;

int: x;//declaration
int: x = 10;//initialisation
int: x = 10, y = 20;//multiple initialisations for the same type

x = 100;//assignment
```

#### Casting
```cs
int: x = 3.14 as int;//casting achieved through 'as'
```

#### Terneries
```cs
int: x = true ? 1 : 3;
```

### • Output and Input
```cs
string: name = input("whats you're name");//input method handles input, string parameter optional
print("hi " + name + "!");//print statement handles output, requires 1 parameter
```

### • Functions
```cs
//general format
Type Name(Type1: parameter1, /*...*/)
{
  //code
}

void Greet()
{
  print("hi");
}

Greet();//function call
```

### • User Defined Type
```cs
class Name
{
  //code
}

//sugar supports classes, enums, interfaces and structs. Declaring any of the previous 3 requires swapping out class for their respective keywords.
```
