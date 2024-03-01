import React, { CSSProperties, useEffect, useState } from "react";
import { DragDropContext, Droppable, Draggable, DraggingStyle, NotDraggingStyle, DropResult } from "react-beautiful-dnd";

import styled from "styled-components";
import { Player } from "../interfaces/Player";

const Xbutton = styled.button`
border: none;
padding: 5px;
color: white;
background-color: black;
border-radius: 5px;
opacity: .5
`

const StyledSpan = styled.span`
  font-size: medium;
`

interface DragNDropRosterProps {
    roster: Player[];
    setRoster: React.Dispatch<React.SetStateAction<Player[]>>;
    deletePlayer: (player: Player) => void;
}

const DragNDropRoster: React.FC<DragNDropRosterProps> = ({ roster, setRoster, deletePlayer }) => {


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


    const reorder = (list: Player[], startIndex: number, endIndex: number) => {
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
        const reorderedItems = reorder(roster, result.source.index, result.destination.index)
        console.log(reorderedItems)
        setRoster(reorderedItems as Player[])

    }


    const getStarterStyle = (isDragging: boolean, draggableStyle: CSSProperties): CSSProperties => ({
        userSelect: 'none',
        padding: 10,
        marginLeft: 'auto',
        marginRight: 'auto',
        maxWidth: '100%',
        marginBottom: '10px',
        borderRadius: '5px',
        background: isDragging ? 'lightgreen' : 'rgb(204, 255, 200)',
        outline: 'outset',
        outlineWidth: '4px',
        outlineColor: 'lightgreen',
        display: 'flex',
        justifyContent: 'space-between',

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
                            {roster.map((player, index) => (

                                <Draggable key={player.player_id} draggableId={player.player_id} index={index}>
                                    {(provided, snapshot) => (
                                        <div key={index}
                                            ref={provided.innerRef}
                                            {...provided.draggableProps}
                                            {...provided.dragHandleProps}
                                            style={index < 5 ? getStarterStyle(snapshot.isDragging, provided.draggableProps.style as CSSProperties) : getBenchStyle(snapshot.isDragging, provided.draggableProps.style as CSSProperties)}>
                                            <StyledSpan>
                                                {player.full_name}
                                            </StyledSpan>
                                            <span className="deletePlayer">
                                                <Xbutton onClick={() => (deletePlayer(player))}>x</Xbutton>
                                            </span>
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

export default DragNDropRoster;