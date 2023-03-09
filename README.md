# Библиотека для работы с конечными полями
Библиотека представляет собой 2 класса: FiniteField и FiniteFieldsElements.

> Все поля подразумеваются конечными. Поля вида $F_{p^n}$ $\simeq$ $F_p[X]/(q)$, где $p$ -- простое число

Первый класс является служебным, хранит в себе всю информацию о поле, с которым работаем (всего 3 переменные $p - характеристика поля, n - степень, q$ - неприводимый многочлен над полем).

Второй представляет собой элементы заданного поля с реализованными операциями над ними.

## 1.1 Cоздание поля
```c#
//Создание поля
int characteristic = 2;
int degree = 2;
FiniteField GF4 = new FiniteField(characteristic, degree, new int[] { 1, 1, 1 }); 
```
## 1.2 Методы поля
### Получение Единичного элемента
```c#
GF4.Create1();
```
### Получение нуля
```c#
GF4.Create0();
```
## 1.3 Создание Элемента 
```c#
//Создание элемента поля
var GF4 = new FiniteField(2,2,new int[]{1,1,1});
FiniteFieldElements element = new FiniteFieldElements(new int[]{2,1},GF4);
```
## 1.4 Операции над элементами
### Сложение
```c#
//Используя поле и элемент созданный ранее
FiniteFieldElements element2 = new FiniteFieldElements(new int[]{1,1},GF4);
var element3 = element+element2;
```
### Вычитание
```c#
//Используя поле и элементы созданный ранее
var element3 = element-element2;
```
### Умножение
```c#
//Используя поле и элементы созданный ранее
var element3 = element*element2;
```
### Возведение в степень
```c#
//Используя поле и элементы созданный ранее
int n = 2;
var resultElement = element.Pow(n);
```
### Деление
```c#
//Используя поле и элементы созданный ранее
var element3 = element/element2;
```
### Нахождение обратного по умножению
```c#
//Используя поле и элементы созданный ранее
var resultElement = element.Reverse();
```
### Нахождение обратного по сложению
```c#
//Используя поле и элементы созданный ранее
var element = -element;
```

