## Quicker

This is a framework is centred to reduce the ammount of boilerplate code written by a Developer in a proyect, and provide a design pattern for develop a profesional software.

Ussually, the main functionality structure to apply in most of cases, is the CRUD. I mean, there is  in EVERY PROJECT... so, why not do a OOP aproach to reduce that boilerplate code in a shot ammount of letters? :)

That is the main propourse of this package, serve a aproach to reduce that, in a few abstract classes that do all the work, letting you to center in the important things. Also, this package will help you in the mapping of classes between layers too, of course!

This package use EntityFrameworkCore and AutoMapper to work, and DataAnnotations for validate by default, but you can use any validator (Like FluentValidator) overriding the ValidateObject virtual method in any class that inherent from OpenService class

Todo:

- Complete the Service abstractions, and test it in the most of cases
- Complete the Controller Web API abstractions, and test it in the most of cases

Posibble added in future, if there are plausible:

- Add abstractions of MVC Controllers
- Add a implementation with more than one Primary key (Max four)
- Add a optional soft delete
- Add a implementation of all in DPT pattern, for a multitenant solution
- Add a pre executing method layer for validation of parameters (Aspect Oriented)
