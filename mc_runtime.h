#ifndef MC_RUNTIME
#define MC_RUNTIME

#include <stdlib.h>
#include <stdint.h>
#include <stddef.h>
#include <string.h>
#include <stdbool.h>
#include <assert.h>

typedef struct {
    size_t  elem_size;
    size_t  cap; // nr of elements, not nr of bytes
    void*   data;
    size_t* refcount;
} table;

void table_insert(table* v, void* elem){
    while (true){
        size_t i;
        for (i = 0; i < v->cap; i += 1){
            if (v->refcount[i] == 0){
                v->refcount[i] = 1;
                memcpy((char*)v->data + (i*v->elem_size), elem, v->elem_size);
                return;
            }
        }
        size_t old_cap = v->cap;
        size_t new_cap = old_cap * 2;
        v->cap = new_cap;
        v->data = realloc(v->data, new_cap*v->elem_size);
        assert(v->data && "failed to reallocate data segment of table");
        v->refcount = (size_t*)realloc(v->refcount, new_cap*sizeof(size_t));
        assert(v->refcount && "failed to reallocate refcount segment of table");
        memset(v->refcount + old_cap, 0, (new_cap - old_cap)*sizeof(size_t));
    }
}

table table_alloc(size_t nr_of_elems, size_t elem_size){
    table t;
    t.cap = nr_of_elems;
    t.elem_size = elem_size;
    t.data = malloc(elem_size*nr_of_elems);
    assert(t.data && "failed to allocate data segment of table");
    t.refcount = (size_t*)calloc(nr_of_elems, sizeof(size_t));
    assert(t.refcount && "failed to allocate refcount segment of table");
    return t;
}

#endif