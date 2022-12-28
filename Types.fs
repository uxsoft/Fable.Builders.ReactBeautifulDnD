module Fable.Builders.ReactBeautifulDnD.Types

open System
open Fable.Core
open Browser.Types
open Feliz

type Position = { x: float; y: float }

type Rect =
    {
      // ClientRect
      top: float
      right: float
      bottom: float
      left: float
      width: float
      height: float
      // DOMRect
      x: float
      y: float
      // Rect
      center: Position }

type Spacing =
    { top: float
      right: float
      bottom: float
      left: float }

type BoxModel =
    {
      // content + padding + border + margin
      marginBox: Rect
      // content + padding + border
      borderBox: Rect
      // content + padding
      paddingBox: Rect
      // content
      contentBox: Rect
      // for your own consumption
      border: Spacing
      padding: Spacing
      margin: Spacing }

// This is an extension of DOMRect and ClientRect

type Id = string
type DraggableId = Id
type DroppableId = Id
type TypeId = Id
type ContextId = Id
type ElementId = Id

[<StringEnum>]
type DroppableMode =
    | Standard
    | Virtual

type DroppableDescriptor =
    { id: DroppableId
      ``type``: TypeId
      mode: DroppableMode }

type DraggableDescriptor =
    { id: DraggableId
      index: float
      // Inherited from Droppable
      droppableId: DroppableId
      // This is technically redundant but it units
      // needing to look up a parent droppable just to get its type
      ``type``: TypeId }

type DraggableOptions =
    { canDragInteractiveElements: bool
      shouldRespectForcePress: bool
      isEnabled: bool }

[<StringEnum>]
type Direction =
    | Horizontal
    | Vertical

let VerticalAxis =
    {| direction = "vertical"
       line = "y"
       start = "top"
       ``end`` = "bottom"
       size = "height"
       crossAxisLine = "x"
       crossAxisStart = "left"
       crossAxisEnd = "right"
       crossAxisSize = "width" |}

let HorizontalAxis =
    {| direction = "horizontal"
       line = "x"
       start = "left"
       ``end`` = "right"
       size = "width"
       crossAxisLine = "y"
       crossAxisStart = "top"
       crossAxisEnd = "bottom"
       crossAxisSize = "height" |}

type Axis =
    | VerticalAxis
    | HorizontalAxis

type ScrollSize =
    { scrollHeight: float
      scrollWidth: float }

type ScrollDifference =
    { value: Position
      // The actual displacement as a result of a scroll is in the opposite
      // direction to the scroll itself. When scrolling down items are displaced
      // upwards. This value is the negated version of the "value"
      displacement: Position }

type ScrollDetails =
    { initial: Position
      current: Position
      // the maximum allowable scroll for the frame
      max: Position
      diff: ScrollDifference }

type Placeholder =
    { client: BoxModel
      tagName: string
      display: string }

type DraggableDimension =
    { descriptor: DraggableDescriptor
      // the placeholder for the draggable
      placeholder: Placeholder
      // relative to the viewport when the drag started
      client: BoxModel
      // relative to the whole page
      page: BoxModel
      // how much displacement the draggable causes
      // this is the size of the marginBox
      displaceBy: Position }

type Scrollable =
    {
      // This is the window through which the droppable is observed
      // It does not change during a drag
      pageMarginBox: Rect
      // Used for comparision with dynamic recollecting
      frameClient: BoxModel
      scrollSize: ScrollSize
      // Whether or not we should clip the subject by the frame
      // Is controlled by the ignoreContainerClipping prop
      shouldClipSubject: bool
      scroll: ScrollDetails }

type PlaceholderInSubject =
    {
      // might not actually be increased by
      // placeholder if there is no required space
      increasedBy: Position option
      placeholderSize: Position
      // max scroll before placeholder added
      // will be null if there was no frame
      oldFrameMaxScroll: Position option }

type DroppableSubject =
    {
      // raw, unchanging
      page: BoxModel
      withPlaceholder: PlaceholderInSubject option
      // The hitbox for a droppable
      // - page margin box
      // - with scroll changes
      // - with any additional droppable placeholder
      // - clipped by frame
      // The subject will be null if the hit area is completely empty
      active: Rect option }

type DroppableDimension =
    { descriptor: DroppableDescriptor
      axis: Axis
      isEnabled: bool
      isCombineEnabled: bool
      // relative to the current viewport
      client: BoxModel
      // relative to the whole page
      isFixedOnPage: bool
      // relative to the page
      page: BoxModel
      // The container of the droppable
      frame: Scrollable option
      // what is visible through the frame
      subject: DroppableSubject }

type DraggableLocation =
    { droppableId: DroppableId
      index: int }

type DraggableIdMap = obj
//{
//    [id: string]: true
//}

type DroppableIdMap = obj
//{
//    [id: string]: true
//}

type DraggableDimensionMap = obj
//{
//    [key: string]: DraggableDimension
//}

type DroppableDimensionMap = obj
//{
//    [key: string]: DroppableDimension
//}

type Displacement =
    { draggableId: DraggableId
      shouldAnimate: bool }

type DisplacementMap = obj
//{
//    [key: string]: Displacement
//}

type DisplacedBy = { value: float; point: Position }

[<StringEnum>]
type VerticalUserDirection =
    | Up
    | Down

[<StringEnum>]
type HorizontalUserDirection =
    | Left
    | Right

type UserDirection =
    { vertical: VerticalUserDirection
      horizontal: HorizontalUserDirection }

// details of the item that is being combined with
type Combine =
    { draggableId: DraggableId
      droppableId: DroppableId }

type DisplacementGroups =
    { all: DraggableId []
      visible: DisplacementMap
      invisible: DraggableIdMap }

type ReorderImpact =
    { ``type``: string //"REORDER"
      destination: DraggableLocation }

type CombineImpact =
    { ``type``: string //"COMBINE"
      whenEntered: UserDirection
      combine: Combine }

type ImpactLocation =
    | ReorderImpact
    | CombineImpact

type Displaced =
    { forwards: DisplacementGroups
      backwards: DisplacementGroups }

type DragImpact =
    { displaced: DisplacementGroups
      displacedBy: DisplacedBy
      at: ImpactLocation option }

type ClientPositions =
    {
      // where the user initially selected
      // This point is not used to calculate the impact of a dragging item
      // It is used to calculate the offset from the initial selection point
      selection: Position
      // the current center of the item
      borderBoxCenter: Position
      // how far the item has moved from its original position
      offset: Position }

type PagePositions =
    { selection: Position
      borderBoxCenter: Position }

// There are two separate modes that a drag can be in
// FLUID: everything is done in response to highly granular input (eg mouse)
// SNAP: items move in response to commands (eg keyboard)
[<StringEnum>]
type MovementMode =
    | FLUID
    | SNAP

type DragPositions =
    { client: ClientPositions
      page: PagePositions }

type DraggableRubric =
    { draggableId: DraggableId
      mode: MovementMode
      source: DraggableLocation }

// Published in onBeforeCapture
// We cannot give more information as things might change in the
// onBeforeCapture responder!
type BeforeCapture =
    { draggableId: DraggableId
      mode: MovementMode }

type DragStart =
    { draggableId: DraggableId
      mode: MovementMode
      ``type``: TypeId
      source: DraggableLocation }



// published when a drag starts
type DragStart2 =
    { draggableId: DraggableId
      mode: MovementMode
      source: DraggableLocation }

type DragUpdate =
    { draggableId: DraggableId
      mode: MovementMode
      source: DraggableLocation
      // may not have any destination (drag to nowhere)
      destination: DraggableLocation option
      // populated when a draggable is dragging over another in combine mode
      combine: Combine option }

[<StringEnum>]
type DropReason =
    | DROP
    | CANCEL

type DropResult =
    { draggableId: DraggableId
      mode: MovementMode
      source: DraggableLocation
      // may not have any destination (drag to nowhere)
      destination: DraggableLocation option
      // populated when a draggable is dragging over another in combine mode
      combine: Combine option
      reason: DropReason }

type ScrollOptions = { shouldPublishImmediately: bool }

// using the draggable id rather than the descriptor as the descriptor
// may change as a result of the initial flush. This means that the lift
// descriptor may not be the same as the actual descriptor. To aunit
// confusion the request is just an id which is looked up
// in the dimension-marshal post-flush
// Not including droppableId as it might change in a drop flush
type LiftRequest =
    { draggableId: DraggableId
      scrollOptions: ScrollOptions }

type Critical =
    { draggable: DraggableDescriptor
      droppable: DroppableDescriptor }

type Viewport =
    {
      // live updates with the latest values
      frame: Rect
      scroll: ScrollDetails }

type LiftEffect =
    { inVirtualList: bool
      effected: DraggableIdMap
      displacedBy: DisplacedBy }

type DimensionMap =
    { draggables: DraggableDimensionMap
      droppables: DroppableDimensionMap }

type DroppablePublish =
    { droppableId: DroppableId
      scroll: Position }

type Published =
    { additions: DraggableDimension []
      removals: DraggableId []
      modified: DroppablePublish [] }

type CompletedDrag =
    { critical: Critical
      result: DropResult
      impact: DragImpact
      afterCritical: LiftEffect }

type IdleState =
    { phase: string //"IDLE"
      completed: CompletedDrag option
      shouldFlush: bool }

type DraggingState =
    { phase: string //"DRAGGING"
      isDragging: bool // true
      critical: Critical
      movementMode: MovementMode
      dimensions: DimensionMap
      initial: DragPositions
      current: DragPositions
      userDirection: UserDirection
      impact: DragImpact
      viewport: Viewport
      afterCritical: LiftEffect
      onLiftImpact: DragImpact
      // when there is a fixed list we want to opt out of this behaviour
      isWindowScrollAllowed: bool
      // if we need to jump the scroll (keyboard dragging)
      scrollJumpRequest: Position option
      // whether or not draggable movements should be animated
      forceShouldAnimate: bool option }

// While dragging we can enter into a bulk collection phase
// During this phase no drag updates are permitted.
// If a drop occurs during this phase, it must wait until it is
// completed before continuing with the drop
// TODO: rename to BulkCollectingState
type CollectingState = DraggingState
//{
//    phase: "COLLECTING"
//}

// If a drop action occurs during a bulk collection we need to
// wait for the collection to finish before performing the drop.
// This is to ensure that everything has the correct index after
// a drop
type DropPendingState = DraggingState
//{
//    phase: "DROP_PENDING"
//    isWaiting: bool
//    reason: DropReason
//}

// An optional phase for animating the drop / cancel if it is needed
type DropAnimatingState =
    { phase: string //"DROP_ANIMATING"
      completed: CompletedDrag
      newHomeClientOffset: Position
      dropDuration: float
      // We still need to render placeholders and fix the dimensions of the dragging item
      dimensions: DimensionMap }

type State =
    | IdleState
    | DraggingState
    | CollectingState
    | DropPendingState
    | DropAnimatingState

type StateWhenUpdatesAllowed =
    | DraggingState
    | CollectingState

type Announce = string -> unit

[<StringEnum; RequireQualifiedAccess>]
type InOutAnimationMode =
    | None
    | Open
    | Close

type ResponderProvided = { announce: Announce }

type OnBeforeCaptureResponder = BeforeCapture -> unit

type OnBeforeDragStartResponder = DragStart -> unit

type OnDragStartResponder = DragStart * ResponderProvided -> unit

type OnDragUpdateResponder = DragUpdate * ResponderProvided -> unit

type OnDragEndResponder = DropResult * ResponderProvided -> unit

type Responders =
    { onBeforeCapture: OnBeforeCaptureResponder option
      onBeforeDragStart: OnBeforeDragStartResponder option
      onDragStart: OnDragStartResponder option
      onDragUpdate: OnDragUpdateResponder option
      // always required
      onDragEnd: OnDragEndResponder }

type StopDragOptions = { shouldBlockNextClick: bool }

type DragActions =
    { drop: StopDragOptions -> unit
      cancel: StopDragOptions -> unit
      isActive: unit -> bool
      shouldRespectForcePress: unit -> bool }

type FluidDragActions =
    { drop: StopDragOptions -> unit
      cancel: StopDragOptions -> unit
      isActive: unit -> bool
      shouldRespectForcePress: unit -> bool
      move: Position -> unit }

type SnapDragActions =
    { drop: StopDragOptions -> unit
      cancel: StopDragOptions -> unit
      isActive: unit -> bool
      shouldRespectForcePress: unit -> bool
      moveUp: unit -> unit
      moveDown: unit -> unit
      moveRight: unit -> unit
      moveLeft: unit -> unit }

type PreDragActions =
    {
      // discover if the lock is still active
      isActive: unit -> bool
      // whether it has been indicated if force press should be respected
      shouldRespectForcePress: unit -> bool
      // lift the current item
      fluidLift: Position -> FluidDragActions
      snapLift: unit -> SnapDragActions
      // cancel the pre drag without starting a drag. Releases the lock
      abort: unit -> unit }

type TryGetLockOptions = { sourceEvent: Event option }

type TryGetLock = DraggableId * (unit -> unit) * TryGetLockOptions -> PreDragActions

type SensorAPI =
    { tryGetLock: TryGetLock
      canGetLock: DraggableId -> bool
      isLockClaimed: unit -> bool
      tryReleaseLock: unit -> unit
      findClosestDraggableId: Event -> DraggableId
      findOptionsForDraggable: DraggableId -> DraggableOptions }

type Sensor = SensorAPI -> unit

type DroppableProvidedProps =
    {
      // used for shared global styles
      ``data-rbd-droppable-context-id``: string
      // Used to lookup. Currently not used for drag and drop lifecycle
      ``data-rbd-droppable-id``: DroppableId }

type DroppableProvided =
    { innerRef: HTMLElement -> unit
      placeholder: ReactElement option
      droppableProps: DroppableProvidedProps }

type DroppableStateSnapshot =
    { isDraggingOver: bool
      draggingOverWith: DraggableId option
      draggingFromThisWith: DraggableId option
      isUsingPlaceholder: bool }

type DropAnimation =
    { duration: float
      curve: string
      moveTo: Position
      opacity: float option
      scale: float option }

//type NotDraggingStyle = {
//    transform: string option
//    transition: "none" option
//}
//
//type DraggingStyle = {
//    position: "fixed"
//    top: float
//    left: float
//    boxSizing: "border-box"
//    width: float
//    height: float
//    transition: "none"
//    transform: string option
//    zIndex: float
//    opacity: number option
//    pointerEvents: "none"
//}

type DraggableProvidedDraggableProps =
    {
      // inline style
//    style: DraggingStyle | NotDraggingStyle option
      // used for shared global styles
      ``data-rbd-draggable-context-id``: string
      ``data-rbd-draggable-id``: string
      onTransitionEnd: TransitionEvent -> unit }

type DraggableProvidedDragHandleProps =
    { ``data-rbd-drag-handle-draggable-id``: DraggableId
      ``data-rbd-drag-handle-context-id``: ContextId
      ``aria-labelledby``: ElementId

      tabIndex: float
      draggable: bool
      onDragStart: DragEvent -> unit }

type DraggableProvided =
    { innerRef: HTMLElement -> unit
      draggableProps: DraggableProvidedDraggableProps
      dragHandleProps: DraggableProvidedDragHandleProps option }

type DraggableStateSnapshot =
    { isDragging: bool
      isDropAnimating: bool
      dropAnimation: DropAnimation option
      draggingOver: DroppableId option
      // the id of a draggable that you are combining with
      combineWith: DraggableId option
      // a combine target is being dragged over by
      combineTargetFor: DraggableId option
      // What type of movement is being done: "FLUID" or "SNAP"
      mode: MovementMode option }

type DraggableChildrenFn = Func<DraggableProvided, DraggableStateSnapshot, DraggableRubric, ReactElement>
