# Language Documentation

## Syntax

### • Variables
Sugar is staticillay typed, thus the type must be specified during variable creation.

#### Creation
```cs
int: x;
```
The type comes first and is followed by a `:` (for variable declarations) which is followed by the name of the variable. Variables can be initiliased to a value and multiple declarations are permitted for variables with the same type.
```cs
int: x = 10;

int: y = x, z;
```

#### Assignment
Assignment takes place through the `=` operator, it is syntactically similar to most languages.
```cs
int: x;

x = 10;
```

#### Properties
Properties in Sugar are similar to that in C#. they can have a `get` and a `set` accessor. a minimum of one of them is required to create a property.
```cs
int: x { get; set; }
```
Accesors can have code within their bodies.
```cs
int: x;

int: X 
{ 
  get
  {
    return x;
  }
  set
  {
    x= value;
  }
}
```

### • Functions
The basic format for function delcrations are as follows
```cs
Type Name(parameters)
{
  //code
}
```

An example:
```cs
void MyFunction(int: x)
{
  //code
}
```

Functions are called like so
```cs
FunctionName(Function Parameters);

MyFunction(0);
```
