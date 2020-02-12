# UbisoftGameLabMcGill_2
Repository for Team McGill #2 for Ubisoft Game Lab Competition

## Coding Best Practices and Requirements
We will be following almost all coding conventions from [here](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/inside-a-program/coding-conventions)

- Every class you write requires a UML diagram due 2 days after a vertical slice on our wednesday meetings. If not provided we will do it together.

- Merging requires unit tests at the discreation of the code reviewers and the authorization of the lead programmer.

- Every class requires a header (in multilign comment form) with the name of the competition, the author(s), the latest modification date and a short description of the class's purpose.

- Every meathod requires a multilign comment roughly following python's numpy convention. These comment blocks require a short description of the method, its purpose, a parameter section, a returned variable section, all the methods called from this method and if need be an exaple of use (for more complicated methods).

   Under the parameter section there must be all parameters with their respective types. The parameter name must be descriptive (it can be long, avoid abreviations) and must contain a 1-3 lign description of the variable in question indented below the "varible: type" header.  

Multilign comments are made like so:
```
/// <summary>
/// Method Name: exampleMethod()
/// 
/// Description:
/// ...
/// 
/// Parameters
/// ----------
/// exampleVariable: string
/// 
///     Decription of variable ...
/// 
/// Return
/// ------
/// exampleReturn: int
/// 
///     Decription of Return
/// 
/// Calls
/// -----
/// helperMethod1()
/// ...
///
/// Example (optional):
///     
///     If you need to show a coding standard example use exampleMethod(exampleVariable)
///     like so, with a string parameter one gets from ...
///     Should expect the following results...
///
/// </summary>
```

## Items that apear in the editor
Are to be serialized fields and contain tool tips like so:
```
[SerializeField]
[Tooltip("WRITE TIP HERE")]
private Object exampleObject;
```
