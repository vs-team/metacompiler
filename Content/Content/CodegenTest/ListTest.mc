Data "nil" : IntList
Data << int >> -> "::"->IntList : IntList  Priority 100
Func "length"-> IntList : Length => << int >> Priority 10

-----------------
length nil => 0


length xs => l
-------------------------
length x::xs => <<l + 1>>
