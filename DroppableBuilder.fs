namespace Fable.Builders.ReactBeautifulDnD

open System
open Fable.Builders.Common
open Fable.Core.JsInterop
open Fable.Builders.ReactBeautifulDnD.Types
open Feliz

module Droppable =
    
    let droppableImport : obj = import "Droppable" "react-beautiful-dnd"
    
    type DroppableBuilder() =
        inherit ReactBuilder(droppableImport)
        
        member _.Run(s: DSLElement) =
            let children = s.Attributes |> List.tryFind (fun (i, v) -> i = "children")
            match children with
            | None -> Interop.reactApi.createElement(droppableImport, createObj s.Attributes, s.Children)
            | Some (_, value) ->
                Interop.reactApi.createElement(droppableImport, createObj s.Attributes, [value |> box |> unbox<ReactElement> ])
        
        [<CustomOperation("droppableId")>] member inline _.droppableId (x: DSLElement, v: DroppableId) = x.attr "droppableId" v
        [<CustomOperation("droppableType")>] member inline _.droppableType (x: DSLElement, v: TypeId) = x.attr "type" v 
        [<CustomOperation("mode")>] member inline _.mode (x: DSLElement, v: DroppableMode) = x.attr "mode" v 
        [<CustomOperation("isDropDisabled")>] member inline _.isDropDisabled (x: DSLElement, v: bool) = x.attr "isDropDisabled" v 
        [<CustomOperation("isCombineEnabled")>] member inline _.isCombineEnabled (x: DSLElement, v: bool) = x.attr "isCombineEnabled" v 
        [<CustomOperation("direction")>] member inline _.direction (x: DSLElement, v: Direction) = x.attr "direction" v 
        [<CustomOperation("ignoreContainerClipping")>] member inline _.ignoreContainerClipping (x: DSLElement, v: bool) = x.attr "ignoreContainerClipping" v 
        [<CustomOperation("renderClone")>] member inline _.renderClone (x: DSLElement, v: DraggableChildrenFn) = x.attr "renderClone" v 
        [<CustomOperation("getContainerForClone")>] member inline _.getContainerForClone (x: DSLElement, v: unit -> ReactElement) = x.attr "getContainerForClone" v
        [<CustomOperation("children")>] member inline _.children (x: DSLElement, v: Func<DroppableProvided, DroppableStateSnapshot, ReactElement>) = x.attr "children" v
