#ifndef MC_RUNTIME
#define MC_RUNTIME

#include <stdlib.h>
#include <stdint.h>
#include <stddef.h>
#include <string.h>
#include <stdbool.h>
#include <assert.h>

typedef struct {
    size_t  cap;
    void*   data;
    size_t* refcount;
} table;

void table_insert(table* v, void* elem, size_t size){
    while (true){
        size_t i;
        for (i = 0; i < v->cap; i += 1){
            if (v->refcount[i] == 0){
                memcpy((char*)v->data + i, elem, size);
                return;
            }
        }
        (void)realloc(v->data, v->cap *= 2);
    }
}

table table_alloc(size_t nr_of_elems, size_t elem_size){
    table t;
    t.cap = nr_of_elems;
    t.data = malloc(elem_size*nr_of_elems);
    assert(t.data);
    t.refcount = (size_t*)calloc(nr_of_elems, sizeof(size_t));
    assert(t.refcount);
    return t;
}

#endif