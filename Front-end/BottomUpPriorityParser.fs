module BottomUpPriorityParser

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
    let res = [ for x in l_nodes do if x.Collapsed |> not then yield x.Element |> fst ]
    res
