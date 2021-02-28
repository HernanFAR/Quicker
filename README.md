# Quicker

## Sobre el framework
> Este es un micro-framework centrado en reducir la cantidad de _boilerplate code_ codificado por un desarrollador en un proyecto, y proveer un patron de arquitectura para desarrollar un software profesional.

Usualmente, una de las funcionalidades basicas a implementar en un proyecto, es el CRUD (Acronimo de Create, Read, Update y Delete), esto normalmente se suele aplicar a cada tabla de una base de datos (Salvo algo excepciones), e incluso, si no es el CRUD en si, es _una parte de el_, por ejemplo, la solo la R, solo CRD, etc. 

Por eso, el proposito de este micro-framework, es brindar una aproximacion en C# para reducir todo ese codigo, a solo unas pocas lineas, mediante POO.

## Dependencias

En esta instante, tiene estas dependencias:

* EntityFrameworkCore: Ya que ayuda con la capa de repositorio
* AutoMapper: Ayuda con la implementacion de un mappeador de clases entre las capas. 
  * Aunque, no es obligatorio usarlo pues puedes _overridear_ las funciones de mappeo para poner tu propio Mapeador. 
* DataAnnotations: Se usa para validar los objectos.
* FluentValidations: Se puede usar para validar objectos, aunque se tiene aca exclusivamente por la excepcion _ValidacionException_. 
  * Por defecto, el sistema usa ValidationException de este paquete, para cuando hay problemas de validacion, __aunque use DataAnnotations__.
  * En caso de que quieras usar solo este paquete, solo debes hacer un override a las funciones de validacion.

## Arquitectura de trabajo que provee

_work in progress_

## Funciones que presta y sus diagramas

_work in progress_

## Pasos futuros

Actualmente, el paquete se encuentra en desarrollo, por lo que aqui abajo hay una lista con los hitos cumplidos, a cumplir, y futuros.

Completados:
- Abstracciones de servicios.
- Diagrama de la arquitectura que te recomienda el framework

Pendiente para la siguiente version menor:
- Agregar loggers en el resto de servicios (Actualmente, solo en CloseService)
- Completar las abstracciones de WebAPI Controllers.
- Agregar una version en ingles de la documentacion de las funciones (Actualmente, es en español)

Pendiente para la 1.0:
- Agregar diagramas y explicaciones de la funcionalidad interna del framework
- Agregar un equivalente en ingles del readme.md

Pendientes para el futuro, si son posibles:

- Agregar abstracciones de MVC Controllers
- Agregar implementaciones con mas de una clave primaria.
- Agregar un _soft delete_ opcional.
- Agregar una implementacion de todo eso, en un patron DTP, para una solucion _multitenant_

## Agradecimientos

- A Alvaro, por ser un apoyo muy grande desde que me converti en desarrollador profesional.

## Inspiraciones

- El pensamiento de _"Si estas repitiendo alguna funcionalidad, algo estas haciendo mal"_.
- A Django, por representar un punto a alcanzar en todo momento, gracias al ORM que ofrece un monton de funciones que ayudan al desarrollador.

## ¿Quieres apoyar?

_work in progress_
