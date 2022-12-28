[<Microsoft.FSharp.Core.AutoOpen>]
module Fable.Builders.ReactBeautifulDnD.DnD

open Fable.Builders.ReactBeautifulDnD.DragDropContext
open Fable.Builders.ReactBeautifulDnD.Draggable
open Fable.Builders.ReactBeautifulDnD.Droppable
open Fable.Core
open Microsoft.FSharp.Core

[<ImportMember("react-beautiful-dnd")>]
let resetServerContext (): unit = jsNative

let DragDropContext = DragDropContextBuilder()
let Droppable = DroppableBuilder()
let Draggable = DraggableBuilder()

module Seq =
    
    [<Emit("Array.from($0)")>]
    let arrayFrom (col: 't array) = jsNative
    
    [<Emit("$2.splice($0, $1)")>]
    let delete (start: int) (deleteCount: int) (col: 't array) : unit = jsNative
    
    [<Emit("$2.splice($0, 0, $1)")>]
    let insert (start: int) (item: 't) (col: 't array) : unit = jsNative
    
    [<Emit("$1.at($0)")>]
    let at (index: int) (col: 't array) : 't = jsNative
    
    let move (source: int) (dest: int) (col: 't array) : 't array =
        let array = arrayFrom col
        let item = at source array
        delete source 1 array
        insert dest item array
        array
        
let DroppableDiv id' children' =
    Droppable {
        droppableId id'
        children (fun provided snapshot ->
            Fable.Builders.React.Html.Div {                                
                ref provided.innerRef
                spread provided.droppableProps
                
                yield! children'
                    
                provided.placeholder.Value
            })
    }
    
let DraggableDiv id' index' children' =
    Draggable {
        draggableId id'
        index index'
        key id'
                
        children (fun provided snapshot rubric ->         
            Fable.Builders.React.Html.Div {
                spread provided.draggableProps
                spread provided.dragHandleProps
                ref provided.innerRef

                yield! children'
            })
    }