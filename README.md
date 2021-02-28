# Quicker

## Sobre el framework
> Este es un micro-framework centrado en reducir la cantidad de _boilerplate code_ codificado por un desarrollador en un proyecto, y proveer un patron de arquitectura para desarrollar un software profesional.

Usualmente, una de las funcionalidades basicas a implementar en un proyecto, es el CRUD (Acronimo de Create, Read, Update y Delete), esto normalmente se suele aplicar a cada tabla de una base de datos (Salvo algo excepciones), e incluso, si no es el CRUD en si, es _una parte de el_, por ejemplo, la solo la R, solo CRD, etc. 

Por eso, el proposito de este micro-framework, es brindar una aproximacion en C# para reducir todo ese codigo, a solo unas pocas lineas, mediante POO.

## Dependencias

En esta instante, tiene estas dependencias:

- EntityFrameworkCore: obligatorio, ya que ayuda con la capa de repositorio
- AutoMapper: Opcional, ayuda con la implementacion de un mappeador de clases entre las capas
- DataAnnotations: Opcional, ayuda con la implementacion de un validador de objetos

## Arquitectura de trabajo que provee

_work in progress_

## Funciones que presta y sus diagramas

_work in progress_

## Pasos futuros

Actualmente, el paquete se encuentra en desarrollo, por lo que aqui abajo hay una lista con los hitos a cumplir, y debajo de ella, los otros hitos a futuro.

Pendiente para la 1.0:

- Agregar diagramas y explicaciones de la funcionalidad interna del framework
- Agregar un equivalente en ingles del readme.md
- Completar las abstracciones de servicios.
- Completar las abstracciones de WebAPI Controllers.

Pendientes para el futuro, si son posibles:

- Agregar abstracciones de MVC Controllers
- Agregar implementaciones con mas de una clave primaria.
- Agregar un _soft delete_ opcional.
- Agregar una implementacion de todo eso, Add a implementation of all in DPT pattern, for a multitenant solution
- Agregar una _capa de preejecucion_ **opcional** para la validacion de parametros (AOP)

## Agradecimientos

- A Alvaro, por ser un apoyo muy grande desde que me converti en desarrollador profesional.

## Inspiraciones

- El pensamiento de _"Si estas repitiendo alguna funcionalidad, algo estas haciendo mal"_.
- A Django, por representar un punto a alcanzar en todo momento, gracias al ORM que ofrece un monton de funciones que ayudan al desarrollador.

## ¿Quieres apoyar?

_work in progress_
