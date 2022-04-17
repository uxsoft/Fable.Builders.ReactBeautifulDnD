namespace Fable.Builders.ReactBeautifulDnD

open System
open Fable.Builders.Common
open Fable.Core
open Fable.React
open Fable.Core.JsInterop
open Fable.Builders.ReactBeautifulDnD.Types

module DragDropContext =

    type DragDropContextBuilder() =
        inherit ReactBuilder(import "DragDropContext" "react-beautiful-dnd")

        [<CustomOperation("onBeforeCapture")>]
        member inline _.onBeforeCapture(x: DSLElement, v: BeforeCapture -> unit) = x.attr "onBeforeCapture" v

        [<CustomOperation("onBeforeDragStart")>]
        member inline _.onBeforeDragStart(x: DSLElement, v: DragStart -> unit) = x.attr "onBeforeDragStart" v

        [<CustomOperation("onDragStart")>]
        member inline _.onDragStart(x: DSLElement, v: Func<DragStart, ResponderProvided, unit>) = x.attr "onDragStart" v

        [<CustomOperation("onDragUpdate")>]
        member inline _.onDragUpdate(x: DSLElement, v: Func<DragUpdate, ResponderProvided, unit>) = x.attr "onDragUpdate" v

        [<CustomOperation("onDragEnd")>]
        member inline _.onDragEnd(x: DSLElement, v: Func<DropResult, ResponderProvided, unit>) = x.attr "onDragEnd" v

        [<CustomOperation("sensors")>]
        member inline _.sensors(x: DSLElement, v: Sensor [] option) = x.attr "sensors" v
