import React, { CSSProperties } from "react";
import { DragDropContext, Droppable, Draggable, DropResult } from "react-beautiful-dnd";

import styled from "styled-components";
import { PropBetStats } from "../interfaces/PropBetStats";

const Xbutton = styled.button`
    border: none;
    color: white;
    background-image: linear-gradient(rgb(40,40,40), rgb(0, 0, 0));
    border-radius: 5px;
    opacity: .9
`

interface MLBPropBetStatsDragNDropProps {
    propBetStats: PropBetStats[];
    setPropBetStats: React.Dispatch<React.SetStateAction<PropBetStats[]>>;
    deletePropBetStat: (stat: PropBetStats) => void;
}

const MLBPropBetStatsDragNDrop: React.FC<MLBPropBetStatsDragNDropProps> = ({ propBetStats, setPropBetStats, deletePropBetStat }) => {


    //useEffect(() => {
    //    const setDragDropRoster = async () => {
    //
    //        let dndRoster = roster.map((player, index) => (
    //            {
    //                id: index.toString(),
    //                player: player.substring(player.indexOf('|') + 1),
    //                salary: player.substring(0, 1),
    //                rosterPlayer: player,
    //                position: index
    //            }
    //        ))
    //        setDragRoster(dndRoster)
    //    }
    //    if (roster) {
    //        setDragDropRoster();
    //    }
    //}, [roster])


    const reorder = (list: PropBetStats[], startIndex: number, endIndex: number) => {
        const result = Array.from(list);
        const [removed] = result.splice(startIndex, 1);
        result.splice(endIndex, 0, removed);

        return result;
    }

    const getListStyle = (isDraggingOver: boolean) => ({
        background: isDraggingOver ? 'lightblue' : 'white',

        maxWidth: '100%',
    })

    const onDragEnd = (result: DropResult) => {
        if (!result.destination) {
            return;
        }
        const reorderedItems = reorder(propBetStats, result.source.index, result.destination.index)
        console.log(reorderedItems)
        setPropBetStats(reorderedItems as PropBetStats[])

    }


    const getStarterStyle = (isDragging: boolean, draggableStyle: CSSProperties): CSSProperties => ({
        userSelect: 'none',
        padding: 5,

        maxWidth: '100%',
        borderRadius: '5px',
        background: isDragging ? 'lightgreen' : 'linear-gradient(rgb(40,40,40), rgb(0, 0, 0))',
        display: 'flex',
        justifyContent: 'space-between',
        alignItems: 'center',
        whiteSpace: 'nowrap',
        ...draggableStyle
    })

    const getBenchStyle = (isDragging: boolean, draggableStyle: CSSProperties): CSSProperties => ({
        userSelect: 'none',
        padding: 5,

        maxWidth: '100%',
        borderRadius: '5px',
        background: isDragging ? 'lightgreen' : 'rgb(6, 6, 9)',
        display: 'flex',
        justifyContent: 'space-between',
        alignItems: 'center',
        whiteSpace: 'nowrap',

        ...draggableStyle
    })

    return (

        <div className="drop-flex">

            <DragDropContext onDragEnd={onDragEnd}>
                <Droppable droppableId="droppable" >
                    {(provided, snapshot) => (
                        <div {...provided.droppableProps} ref={provided.innerRef} style={getListStyle(snapshot.isDraggingOver)}>
                            {propBetStats.map((stat, index) => (

                                <Draggable key={index} draggableId={stat.label} index={index}>
                                    {(provided, snapshot) => (
                                        <div key={index}
                                            ref={provided.innerRef}
                                            {...provided.draggableProps}
                                            {...provided.dragHandleProps}
                                            style={getStarterStyle(snapshot.isDragging, provided.draggableProps.style as CSSProperties)}>
                                            <div className="text-in-box" >
                                                {stat.label}
                                            </div>
                                            <div>
                                                <Xbutton onClick={() => (deletePropBetStat(stat))}>x</Xbutton>
                                            </div>
                                        </div>
                                    )}
                                </Draggable>
                            ))}
                            {provided.placeholder}
                        </div>)}
                </Droppable>
            </DragDropContext>
        </div>
    )
}

export default MLBPropBetStatsDragNDrop;