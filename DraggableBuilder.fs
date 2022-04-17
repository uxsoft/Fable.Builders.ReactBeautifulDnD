namespace Fable.Builders.ReactBeautifulDnD

open Fable.Builders.Common
open Fable.Core
open Fable.React
open Fable.Core.JsInterop
open Fable.Builders.ReactBeautifulDnD.Types
open Feliz

module Draggable =
    let draggableImport : obj = import "Draggable" "react-beautiful-dnd"
    
    type DraggableBuilder() =
        inherit ReactBuilder(draggableImport)
        member _.Run(s: DSLElement) =
            let children = s.Attributes |> List.tryFind (fun (i, v) -> i = "children")
            match children with
            | None -> Interop.reactApi.createElement(draggableImport, createObj s.Attributes, s.Children)
            | Some (_, value) ->
                Interop.reactApi.createElement(draggableImport, createObj s.Attributes, [value |> box |> unbox<ReactElement> ])
                
        [<CustomOperation("draggableId")>] member inline _.draggableId (x: DSLElement, v: DraggableId) = x.attr "draggableId" v
        [<CustomOperation("index")>] member inline _.index (x: DSLElement, v: int) = x.attr "index" v
        [<CustomOperation("children")>] member inline _.children (x: DSLElement, v: DraggableChildrenFn) = x.attr "children" v
        [<CustomOperation("isDragDisabled")>] member inline _.isDragDisabled (x: DSLElement, v: bool) = x.attr "isDragDisabled" v 
        [<CustomOperation("disableInteractiveElementBlocking")>] member inline _.disableInteractiveElementBlocking (x: DSLElement, v: bool) = x.attr "disableInteractiveElementBlocking" v
        [<CustomOperation("shouldRespectForcePress")>] member inline _.shouldRespectForcePress (x: DSLElement, v: bool) = x.attr "shouldRespectForcePress" v