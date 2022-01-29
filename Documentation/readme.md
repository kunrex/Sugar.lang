# Language Documentation

## Syntax

### • Variables

#### Creation
```cs
//variable creation is similar to that in C# or JAVA, however the type must be followed by a ':'
int: x;

//variables can be initialised and multi variable declaration with the same type is allowed
string: x = "hello world!";

int: x, y = 10;

//using the 'var' keyword for the type will lead to the the type being detected during compile time.
var: boolean = true;
```

#### Assignment
```cs
//Assignment takes place through the `=` operator
int: x;

x = 10;
```

#### Properties
```cs
//properties in Sugar are similar to that in C#. 
//properties have a `get` and a `set` accessor. A minimum of one accessor is required to create a property. 

//accessors must go in a '{}' following the property name. 

//like variables, property types must be followed with a ':' 
//unlike variables, properties cannot be declared with the 'var' keyword.
string: name { get; set; }

int: x;

int: X 
{ 
  get
  {
    return x;
  }
  set
  {
    x = value;
  }
}
```

### • Functions

#### Declarations
```cs
//function delecrations are similar to that in C# or JAVA. 
//The type comes first, followed by the name of the function.

//A function cannot have a return type of 'var'

//function parameters go in '()' after the function name, they are split using a ','
//parameters can have default values
void MyFunction(int: x, string: y = "hello")
{
  
}
```

#### Function Calls
```cs
//calling functions
MyFunction(10);//since 'y' had a default value it is not nececarry to provide one during function call
```

### • Interactivity

#### Output
```cs
//printing values is handled using the 'print' function. It requires one parameter.
print(10);
```

#### Input
```cs
//input is handled using the 'input' function. 
//It may or may not be provided with a string parameter.
string: name = input("whats your name?");
```

### Conditions

#### If Conditions
```cs
//if conditions include the 'if' and the 'else' keywords

//the body of the condition goes in a `{}` after the condition itself
//if no '{}' is provided, the first line is taken as the body of the condition.
int: x = 10;
if(x == 10)
{
  print("x is 10");
}
else if(x == 20)
{
  print("x is 20");
}
else
  print("x is not 10 or 20");
```

#### Switch Statements
```cs
//the case keyword is used to check a value. 
//the default keyword is used to signify a body that is executed if none of the cases are true
//fallthroughs are permitted.

//the body of a case is present in a '{}' which is then followed by a break
//if no '{}' is given, the first line is taken as the body
int: x = 10;
switch(10)
{
  case 10:
    {
      print("x is 10");
    }
    break;
  case 20:
    {
      print("x is 20");
    }
    break;
  default:
    print("x is not 10 or 20");
    break;
}
```

the above switch statement and if else chain are functionally similar

### Loops

#### While
```cs
//a while loop is used to run a block of code while a condition is true

//the block of the loop goes in a '{}' following the condition of the loop
//if no '{}' is given, the first line after the condition is taken as the body of the while loop

int: x = 10;
while(x > 5)
{
  print(x);
  x--;
}
```

#### Do While 
```cs
//a do while loop is similar to a while but is exit controlled when the while loop is entry controlled

int: x = 10;
do
{
  print(x);
  x--;
}
while(x > 5)
```

#### For 
```cs
//a for loop also runs until a condition is satsified but provides extra functionality

for(int: x = 10; x > 5; x--)
{
  print(x);
}
```

The above while, do while and for loop are functionally similar
