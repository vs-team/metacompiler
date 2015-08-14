module BottomUpPriorityParser
(*
    The BottomUpPriorityParser is responsible for prioritizing a list of expressions into possibly shorter lists of expressions and eliminating ambigious expressions.
*)


(*
    A node consists of 4 attributes: it's Element, a flag if this Node has been collapsed yet and a Left and Right children.

    The Collapsed Flag will make sure that a Node does not get collapsed twice.
*)

[<CustomEquality; CustomComparison>]
type Node<'a when 'a : equality and 'a : comparison> = 
  { 
    mutable Element   : 'a
    mutable Collapsed : bool
    mutable Left      : Option<Node<'a>>
    mutable Right     : Option<Node<'a>>
  }
  with
    member this.Skip() =
      match this.Left with
      | None -> ()
      | Some left ->
        left.Right <- this.Right
      match this.Right with
      | None -> ()
      | Some right ->
        right.Left <- this.Left
    member this.LeftForced = 
      match this.Left with
      | None -> failwith "Cannot lookup left element"
      | Some x -> x
    member this.RightForced = 
      match this.Right with
      | None -> failwith "Cannot lookup right element"
      | Some x -> x
    (*
        The collapse function will make sure that every element has the same amount of left and right arguments as its ariety given the left ariety and right arity.
        The function will return the merged expression, merging is done by the given function f.
        This is done by iterating over every left and right element and assigning those elements to the either left or right element.
        The iterations are stopped once the amount of elements assigned to this Node is equal to its ariety.

        It is important to note that this function will give an error if it is not possible to satisfy this expression's ariety.
        If the operator '+' has a left and right ariety of 1 then the expression '+ 2' will not be valid because it's left ariety is not satisfied.
    *)
    member this.Collapse (leftCount:int) (rightCount:int) f =
      let mutable left = []
      if leftCount > 0 then
        let mutable pl = this.LeftForced
        for i = 1 to leftCount do
          pl.Collapsed <- true
          left <- pl.Element :: left
          if pl.Left <> None then
            let pl' = pl.LeftForced
            pl.Skip()
            pl <- pl'
      let mutable right = []
      if rightCount > 0 then
        let mutable pr = this.RightForced
        for i = 1 to rightCount do
          pr.Collapsed <- true
          right <- pr.Element :: right
          if pr.Right <> None then
            let pr' = pr.RightForced
            pr.Skip()
            pr <- pr'
      f this.Element (left |> List.rev) (right |> List.rev)
    override x.Equals(yobj) =
        match yobj with
        | :? Node<'a> as y -> (x.Element = y.Element)
        | _ -> false
 
    override x.GetHashCode() = hash x.Element
    interface System.IComparable with
      member x.CompareTo yobj =
          match yobj with
          | :? Node<'a> as y -> compare x.Element y.Element
          | _ -> invalidArg "yobj" "cannot compare values of different types"

(*
    The prioritize function accepts a list of Expressions, a function returning the left and right ariety of an expression,
    a function returning the priority of an expression and a function that enables merging multiple expressions into one.

    The prioritizing is done by comparing the priority of multiple expressions and comparing these to it's neighbours.
    If an expression takes a higher priority than it's neighbour, and this expression has an ariety of 1 or more, the element and it's neighbour will be merged depending on the ariety.

    Example:

    Given:
     - the expression e : 3 + 1 * 3
     - the operator '+' has a left ariety of 1 , a right ariety of 1 and a priority of 1
     - the operator '*' has a left ariety of 1 , a right ariety of 1 and a priority of 2

    If e were to be parsed into a list it could be represented as the following:

    {literal(3); operator('+', 1, 1, 1); literal(1); operator('*', 1, 1, 2); literal(3)}

    Sorted on priority:

    {operator('*', 1, 1, 2); operator('+', 1, 1, 1); literal(3); literal(1); literal(3)}

    Iterating over the sorted list will start of at the '*' operator.
    We first check if this expression has an ariety that is greater than 0.
    In this case '*' has a left and right ariety of 1.
    If this expression is not collapsed yet, we will collapse ( see Node.Collapse ) this expression.
    
    After this step there will be two not-collapsed elements left in the list of expressions.
    
    The next element in the list of sorted expressions will be the '+' operator.
    Now, we simply collapse this expression and we will end up with a prioritized list of expressions.

    Note that the prioritized list is shorter than it's original. This is because the new List will only contain expressions that have not collapsed.
*)
let prioritize (l:List<'a>) ariety priority merge =
  match l with
  | [] -> failwith "Cannot prioritize empty list"
  | [x] -> [x]
  | x::xs ->
    let l = l |> List.mapi (fun i x -> x,i)
    let l_by_priority = l |> List.sortBy priority |> List.rev 
    let l_nodes = [| for x in l do yield { Element = x; Collapsed = false; Left = None; Right = None } |]
    l_nodes.[l_nodes.Length - 1].Left <- Some(l_nodes.[l_nodes.Length - 2])
    l_nodes.[0].Right <- Some(l_nodes.[1])
    for i = 1 to l_nodes.Length - 2 do
      l_nodes.[i].Left <- Some(l_nodes.[i-1])
      l_nodes.[i].Right <- Some(l_nodes.[i+1])
    let mutable l_nodes_by_element = seq{for n in l_nodes -> n.Element, n} |> Map.ofSeq
    for x,i in l_by_priority do
      let leftCount,rightCount = ariety x
      let x_node = l_nodes_by_element.[x,i]
      if (leftCount > 0 || rightCount > 0) && x_node.Collapsed |> not then
        let newNode = x_node.Collapse leftCount rightCount merge
        x_node.Element <- newNode
      //[ for x in l_nodes do if x.Collapsed |> not then yield x.Element |> fst ] |> printfn "%A"
    let res = [ for x in l_nodes do if x.Collapsed |> not then yield x.Element |> fst ]
    res
