import React, { CSSProperties, useEffect, useState } from "react";
import { DragDropContext, Droppable, Draggable, DraggingStyle, NotDraggingStyle, DropResult } from "react-beautiful-dnd";

import styled from "styled-components";
import { Player } from "../interfaces/Player";
import { PropBetStats } from "../interfaces/PropBetStats";

const Xbutton = styled.button`
border: none;
color: white;
background-color: black;
border-radius: 3px;
opacity: .9
`

const StyledSpan = styled.div`
  color: rgb(200,100,150);
  font-size: medium;
  padding: 5px;
`

interface PropBetStatsDragNDropProps {
    propBetStats: PropBetStats[];
    setPropBetStats: React.Dispatch<React.SetStateAction<PropBetStats[]>>;
    deletePropBetStat: (stat: PropBetStats) => void;
}

const PropBetStatsDragNDrop: React.FC<PropBetStatsDragNDropProps> = ({ propBetStats, setPropBetStats, deletePropBetStat }) => {


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
        width: 'auto',
        marginLeft: 'auto',
        marginRight: 'auto',
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
        
        maxWidth: '100%',
        borderRadius: '3px',
        background: isDragging ? 'lightgreen' : 'rgb(0,0,20)',
        outline: 'outset',
        outlineWidth: '2px',
        outlineColor: 'lightgreen',
        display: 'flex',
        justifyContent: 'space-between',
        alignItems: 'center',
        whiteSpace: 'nowrap',
        ...draggableStyle
    })

    const getBenchStyle = (isDragging: boolean, draggableStyle: CSSProperties): CSSProperties => ({
        userSelect: 'none',
        padding: 10,
        maxWidth: '100%',
        marginLeft: 'auto',
        marginRight: 'auto',
        marginBottom: '10px',
        borderRadius: '5px',
        background: isDragging ? 'lightblue' : 'rgb(238,238,238)',
        outline: 'outset',
        outlineWidth: '4px',
        outlineColor: 'rgb(210,210,210)',
        display: 'flex',
        justifyContent: 'space-between',

        ...draggableStyle
    })

    return (

        <div className="drop-flex">

            <DragDropContext onDragEnd={onDragEnd}>
                <Droppable droppableId="droppable" >
                    {(provided, snapshot) => (
                        <div {...provided.droppableProps} ref={provided.innerRef} style={getListStyle(snapshot.isDraggingOver)}>
                            {propBetStats.map((stat, index) => (

                                <Draggable key={stat.label} draggableId={stat.label} index={index}>
                                    {(provided, snapshot) => (
                                        <div key={index}
                                            ref={provided.innerRef}
                                            {...provided.draggableProps}
                                            {...provided.dragHandleProps}
                                            style={index < 5 ? getStarterStyle(snapshot.isDragging, provided.draggableProps.style as CSSProperties) : getBenchStyle(snapshot.isDragging, provided.draggableProps.style as CSSProperties)}>
                                            <StyledSpan>
                                                {stat.label}
                                            </StyledSpan>
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

export default PropBetStatsDragNDrop;