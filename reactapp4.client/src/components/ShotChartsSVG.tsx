import React, { useState, useEffect } from 'react';
import * as d3 from 'd3';
import { Shot } from '../interfaces/Shot';


interface ShotChartsSVGProps {
    shotsData: Shot[];
    isGameChart: boolean;
}

const ShotChartSVG: React.FC<ShotChartsSVGProps> = ({ shotsData, isGameChart }) => {

    const [myPlot, setMyPlot] = useState(isGameChart ? "myPlot" : "myPlot2");
    //let [chartTitle, setCharTitle] = useState(isGameChart ? "SEASON SHOT CHART" : "GAME SHOT CHART");

    /*
    useEffect(() => {
        const getPlot = async() => {
            console.log(game);

            if (game) {
                setMyPlot('myPlot2');
                setCharTitle("GAME SHOT CHART");
            } else {
                setMyPlot('myPlot');
                setCharTitle("SEASON SHOT CHART");
            }
            getPlot();
        }
    }, [])*/

    useEffect(() => {
        const getSVG = async () => {

            const data = [];
            const dataMadeShots = [];
            const dataMissedShots = [];


            for (let i = 0; i < shotsData.length; i++) {
                //data.push([totalShotsArray.resultSets[0].rowSet[i][17], totalShotsArray.resultSets[0].rowSet[i][18]]);
                if (shotsData[i].shot_made_flag === "1") {
                    dataMadeShots.push([shotsData[i].loc_x, shotsData[i].loc_y])
                } else {
                    dataMissedShots.push([shotsData[i].loc_x, shotsData[i].loc_y]);
                }
            }

            const xSize = 620;
            const ySize = 570;
            const xHalf = -350;
            const xPosHalf = 380;
            const xMargin = 120;
            const yMargin = 60;
            const width = xSize - xMargin
            const height = ySize - yMargin
            const halfWidth = xHalf + xMargin;
            const halfPosWidth = xPosHalf - xMargin;
            const svg = d3.select(`#${myPlot}`)
                .append("svg")
                .append("g")
                .attr("transform", "translate(" + halfPosWidth + ", " + yMargin + ")", "class", "graph-svg-component")
                .style('color', 'white');

            d3.select(`#${myPlot}`).selectAll("text").remove()
            // X Axis
            const x = d3.scaleLinear()
                .domain([-250, 250])
                .range([0, width])


            svg.append("g")
                .attr("transform", "translate(-250, -52.5)")
                .call(d3.axisBottom(x))
                .style('color', 'white');

            /*
            if (shotsData !== 'No shots attempted') {
                svg.append("text")
                  .attr("x", 260)
                  .attr("y", 70)
                  .text(`pts: ${parseFloat(boxData[0].pts).toFixed(2)}`)
                  .style("text-anchor", "left")
                  .style("font-size", "15px")
                  .style('fill', 'white')

                svg.append("text")
                  .attr("x", 260)
                  .attr("y", 90)
                  .text(`min: ${parseFloat(boxData[0].min).toFixed(2)}`)
                  .style("text-anchor", "left")
                  .style("font-size", "15px")
                  .style('fill', 'white')

                svg.append("text")
                  .attr("x", 260)
                  .attr("y", 110)
                  .text(`fga: ${parseFloat(boxData[0].fga).toFixed(2)}`)
                  .style("text-anchor", "left")
                  .style("font-size", "15px")
                  .style('fill', 'white')

                svg.append("text")
                  .attr("x", 260)
                  .attr("y", 130)
                  .text(`fgm: ${parseFloat(boxData[0].fgm).toFixed(2)}`)
                  .style("text-anchor", "left")
                  .style("font-size", "15px")
                  .style('fill', 'white')  

                svg.append("text")
                  .attr("x", 260)
                  .attr("y", 150)
                  .text(`fgp: ${parseFloat(boxData[0].fg_pct).toFixed(2)}`)
                  .style("text-anchor", "left")
                  .style("font-size", "15px")
                  .style('fill', 'white')
                svg.append("text")
                  .attr("x", 260)
                  .attr("y", 170)
                  .text(`fta: ${parseFloat(boxData[0].fta).toFixed(2)}`)
                  .style("text-anchor", "left")
                  .style("font-size", "15px")
                  .style('fill', 'white')
                svg.append("text")
                  .attr("x", 260)
                  .attr("y", 190)
                  .text(`ftm: ${parseFloat(boxData[0].ftm).toFixed(2)}`)
                  .style("text-anchor", "left")
                  .style("font-size", "15px")
                  .style('fill', 'white')    
                svg.append("text")
                  .attr("x", 260)
                  .attr("y", 210)
                  .text(`ftp: ${parseFloat(boxData[0].ft_pct).toFixed(2)}`)
                  .style("text-anchor", "left")
                  .style("font-size", "15px")
                  .style('fill', 'white')
                svg.append("text")
                  .attr("x", 260)
                  .attr("y", 230)
                  .text(`fg3a: ${parseFloat(boxData[0].fg3m).toFixed(2)}`)
                  .style("text-anchor", "left")
                  .style("font-size", "15px")
                  .style('fill', 'white')

                svg.append("text")
                  .attr("x", 260)
                  .attr("y", 250)
                  .text(`fg3m: ${parseFloat(boxData[0].fg3a).toFixed(2)}`)
                  .style("text-anchor", "left")
                  .style("font-size", "15px")
                  .style('fill', 'white')

                svg.append("text")
                  .attr("x", 260)
                  .attr("y", 270)
                  .text(`fg3p: ${parseFloat(boxData[0].fg3_pct).toFixed(2)}`)
                  .style("text-anchor", "left")
                  .style("font-size", "15px")
                  .style('fill', 'white')

                svg.append("text")
                  .attr("x", 260)
                  .attr("y", 290)
                  .text(`reb: ${parseFloat(boxData[0].reb).toFixed(2)}`)
                  .style("text-anchor", "left")
                  .style("font-size", "15px")
                  .style('fill', 'white')

                svg.append("text")
                  .attr("x", 260)
                  .attr("y", 310)
                  .text(`ast: ${parseFloat(boxData[0].ast).toFixed(2)}`)
                  .style("text-anchor", "left")
                  .style("font-size", "15px")
                  .style('fill', 'white')

                svg.append("text")
                  .attr("x", 260)
                  .attr("y", 330)
                  .text(`stl: ${parseFloat(boxData[0].stl).toFixed(2)}`)
                  .style("text-anchor", "left")
                  .style("font-size", "15px")
                  .style('fill', 'white')

                svg.append("text")
                  .attr("x", 260)
                  .attr("y", 350)
                  .text(`tov: ${parseFloat(boxData[0].to).toFixed(2)}`)
                  .style("text-anchor", "left")
                  .style("font-size", "15px")
                  .style('fill', 'white')

                svg.append("text")
                  .attr("x", 260)
                  .attr("y", 370)
                  .text(`blk: ${parseFloat(boxData[0].blk).toFixed(2)}`)
                  .style("text-anchor", "left")
                  .style("font-size", "15px")
                  .style('fill', 'white')

                svg.append("text")
                  .attr("x", 260)
                  .attr("y", 390)
                  .text(`+/-: ${parseFloat(boxData[0]['+/-']).toFixed(2)}`)
                  .style("text-anchor", "left")
                  .style("font-size", "15px")
                  .style('fill', 'white')
            } else {
              svg.append("text")
              .attr("x", 260)
              .attr("y", 50)
              .text(`No shots attempted`)
            }    
            */
            svg.append("line")
                .attr("x1", 60)
                .attr("x2", 60)
                .attr("y1", -52.5)
                .attr("y2", 137.5)
                .attr("stroke", "white")
                .attr("stroke-width", "2")

            svg.append("line")
                .attr("x1", -250)
                .attr("x2", 250)
                .attr("y1", 418)
                .attr("y2", 418)
                .attr("stroke", "white")
                .attr("stroke-width", "2")

            svg.append("line")
                .attr("x1", -60)
                .attr("x2", -60)
                .attr("y1", -52.5)
                .attr("y2", 137.5)
                .attr("stroke", "white")
                .attr("stroke-width", "2")

            svg.append("line")
                .attr("x1", -80)
                .attr("x2", 80)
                .attr("y1", 137.5)
                .attr("y2", 137.5)
                .attr("stroke", "white")
                .attr("stroke-width", "2")

            svg.append("line")
                .attr("x1", -80)
                .attr("x2", -80)
                .attr("y1", -52.5)
                .attr("y2", 137.5)
                .attr("stroke", "white")
                .attr("stroke-width", "2")

            svg.append("line")
                .attr("x1", 80)
                .attr("x2", 80)
                .attr("y1", -52.5)
                .attr("y2", 137.5)
                .attr("stroke", "white")
                .attr("stroke-width", "2")

            svg.append("line")
                .attr("x1", -219)
                .attr("x2", -221)
                .attr("y1", -52.5)
                .attr("y2", 87.5)
                .attr("stroke", "white")
                .attr("stroke-width", "2")

            svg.append("line")
                .attr("x1", 219)
                .attr("x2", 221)
                .attr("y1", -52.5)
                .attr("y2", 87.5)
                .attr("stroke", "white")
                .attr("stroke-width", "2")
            svg.append("line")
                .attr("x1", -30)
                .attr("x2", 30)
                .attr("y1", -8.25)
                .attr("y2", -8.25)
                .attr("stroke", "orange")
                .attr("stroke-width", "5")

            svg.append("line")
                .attr("x1", 80)
                .attr("x2", 87)
                .attr("y1", 100)
                .attr("y2", 100)
                .attr("stroke", "white")
                .attr("stroke-width", "4")
            svg.append("line")
                .attr("x1", -80)
                .attr("x2", -87)
                .attr("y1", 100)
                .attr("y2", 100)
                .attr("stroke", "white")
                .attr("stroke-width", "4")

            svg.append("line")
                .attr("x1", 80)
                .attr("x2", 87)
                .attr("y1", 70)
                .attr("y2", 70)
                .attr("stroke", "white")
                .attr("stroke-width", "4")
            svg.append("line")
                .attr("x1", -80)
                .attr("x2", -87)
                .attr("y1", 70)
                .attr("y2", 70)
                .attr("stroke", "white")
                .attr("stroke-width", "4")

            svg.append("line")
                .attr("x1", 80)
                .attr("x2", 87)
                .attr("y1", 40)
                .attr("y2", 40)
                .attr("stroke", "white")
                .attr("stroke-width", "4")
            svg.append("line")
                .attr("x1", -80)
                .attr("x2", -87)
                .attr("y1", 40)
                .attr("y2", 40)
                .attr("stroke", "white")
                .attr("stroke-width", "4")


            svg.append("line")
                .attr("x1", 80)
                .attr("x2", 87)
                .attr("y1", 30)
                .attr("y2", 30)
                .attr("stroke", "white")
                .attr("stroke-width", "4")
            svg.append("line")
                .attr("x1", -80)
                .attr("x2", -87)
                .attr("y1", 30)
                .attr("y2", 30)
                .attr("stroke", "white")
                .attr("stroke-width", "4")

            svg.append("line")
                .attr("x1", -250)
                .attr("x2", -250)
                .attr("y1", -52)
                .attr("y2", 418)
                .attr("stroke", "white")
            // Y Axis
            const y = d3.scaleLinear()
                .domain([-52.5, 418])
                .range([-52.5, 418]);

            svg.append("g")
                .attr("transform", "translate(250, 0)")
                .call(d3.axisLeft(y));

            // Dots
            d3.select(`#${myPlot}`).selectAll("circle").remove()

            svg.append("circle")
                .attr("cx", 0)
                .attr("cy", 0)
                .attr("r", 7.5)
                .attr("stroke", "orange")
                .style("stroke-width", 2)
                .style("fill", "none");

            svg.append("circle")
                .attr("cx", 0)
                .attr("cy", 137.5)
                .attr("r", 60)
                .style("opacity", .2)
                .attr("stroke", "white")
                .style("fill", "rgb(65,65,65)");
            svg.append("circle")
                .attr("cx", 0)
                .attr("cy", 418)
                .attr("r", 60)
                .style("opacity", .2)
                .attr("stroke", "white")
                .style("fill", "rgb(65,65,65)");

            svg.append("circle")
                .attr("cx", 0)
                .attr("cy", 418)
                .attr("r", 20)
                .attr("stroke", "white")
                .style("fill", "none");

            const arcGenerator2 = d3.arc()
                .outerRadius(61)
                .innerRadius(59)
                .startAngle(0)
                .endAngle(2 * Math.PI);
            const halfCourtCircle = svg.append("path")
                .attr("transform", "translate(0, 418)")
                .attr("fill", "white")
                .attr("d", arcGenerator2());

            const freeThrowCircle = svg.append("path")
                .attr("transform", "translate(0, 137.5)")
                .attr("fill", "white")
                .attr("d", arcGenerator2());
            /*svg.append("circle")
              .attr("cx", 0)
              .attr("cy", 0)
              .attr("r", 235)
              .attr("stroke", "white")
              .style("fill", "none");
            */
            const arcGenerator = d3.arc()
                .outerRadius(238.5)
                .innerRadius(236.5)
                .startAngle(Math.PI / 2 + 0.37855)
                .endAngle(Math.PI * 3 / 2 - 0.37855);
            const threeLine = svg.append("path")
                .attr("transform", "translate(0, 0)")
                .attr("fill", "white")
                .attr("d", arcGenerator())
            svg.append('g')
                .selectAll("dot")
                .data(dataMissedShots).enter()
                .append("circle")
                .attr("cx", function (d: number[]) { return d[0] })
                .attr("cy", function (d:number[]) { return d[1] })
                .attr("r", 2)
                .style("opacity", .7)
                .style("fill", "rgba(64, 154, 244, 0.5)");

            svg.append('g')
                .selectAll("dot")
                .data(dataMadeShots).enter()
                .append("circle")
                .attr("cx", function (d: number[]) { return d[0] })
                .attr("cy", function (d: number[]) { return d[1] })
                .style("opacity", .8)
                .attr("r", 2)
                .style("fill", "rgb(191, 255, 0)");
        }
        if (shotsData) {
            console.log(shotsData)
            //console.log(boxData)
            getSVG();
        }
    }, [shotsData])

    return (
        <div>
            <svg id={myPlot}
                style={{
                    height: 470,
                    width: 520,
                    margin: "auto",
                    display: "block",
                    backgroundColor: "black",
                }}
            >
            </svg>

        </div>
    );
}

export default ShotChartSVG;
/*
   console.log(shotsData);
   console.log(playerid);
   console.log(boxData);
   console.log(season);
   let myPlot = 'myPlot';
   let chartTitle = "SEASON SHOT CHART";

   const data = [];
   const dataMadeShots = [];
   const dataMissedShots = [];


   for (let i = 0; i < shotsData.length; i++) {
       //data.push([totalShotsArray.resultSets[0].rowSet[i][17], totalShotsArray.resultSets[0].rowSet[i][18]]);
       if (shotsData[i].shot_made_flag === "1") {
         dataMadeShots.push([shotsData[i].loc_x, shotsData[i].loc_y])
       } else {
         dataMissedShots.push([shotsData[i].loc_x, shotsData[i].loc_y]);
       }
   }
   if (boxData) {
       console.log(boxData);
       const xSize = 600; 
       const ySize = 570;
       const xHalf = -350;
       const xPosHalf = 350;
       const xMargin = 100;
       const yMargin = 100;
       const width = xSize - xMargin
       const height = ySize - yMargin
       const halfWidth = xHalf + xMargin;
       const halfPosWidth = xPosHalf - xMargin;  
       
       const svg = d3.select(`#${myPlot}`)
         .append("svg")
         .append("g")
         .attr("transform","translate(" + halfPosWidth + ", " + yMargin + ")");
       
       d3.select(`#${myPlot}`).selectAll("text").remove()
         // X Axis
       const x = d3.scaleLinear()
         .domain([-250, 250])
         .range([0, width]);

       svg.append("g")
         .attr("transform", "translate(-250, -52.5)")
         .call(d3.axisBottom(x));

       svg.append("text")
         .attr("x", 0)
         .attr("y", -65)
         .text(`${chartTitle}`)
         .style("text-anchor", "middle")
         .style("font-size", "40px")
         .style('fill', 'chartreuse')
       
       svg.append("text")
         .attr("x", 0)
         .attr("y", -65)
         .text(`${chartTitle}`)
         .style("text-anchor", "middle")
         .style("font-size", "40px")
         .style('fill', 'chartreuse')
       
       
       svg.append("text")
         .attr("x", 260)
         .attr("y", 70)
         .text(`PPG: ${parseFloat(boxData[0].pts).toFixed(2)}`)
         .style("text-anchor", "left")
         .style("font-size", "20px")
         .style('fill', 'chartreuse')

       svg.append("text")
         .attr("x", 260)
         .attr("y", 90)
         .text(`MIN: ${parseFloat(boxData[0].min).toFixed(2)}`)
         .style("text-anchor", "left")
         .style("font-size", "20px")
         .style('fill', 'chartreuse')

       svg.append("text")
         .attr("x", 260)
         .attr("y", 110)
         .text(`FGA: ${parseFloat(boxData[0].fga).toFixed(2)}`)
         .style("text-anchor", "left")
         .style("font-size", "20px")
         .style('fill', 'chartreuse')

       svg.append("text")
         .attr("x", 260)
         .attr("y", 130)
         .text(`FGM: ${parseFloat(boxData[0].fgm).toFixed(2)}`)
         .style("text-anchor", "left")
         .style("font-size", "20px")
         .style('fill', 'chartreuse')  

       svg.append("text")
         .attr("x", 260)
         .attr("y", 150)
         .text(`FGP: ${parseFloat(boxData[0].fg_pct).toFixed(2)}`)
         .style("text-anchor", "left")
         .style("font-size", "20px")
         .style('fill', 'chartreuse')
       
       svg.append("text")
         .attr("x", 260)
         .attr("y", 170)
         .text(`FTA: ${parseFloat(boxData[0].fta).toFixed(2)}`)
         .style("text-anchor", "left")
         .style("font-size", "20px")
         .style('fill', 'chartreuse')
       
       svg.append("text")
         .attr("x", 260)
         .attr("y", 190)
         .text(`FTM: ${parseFloat(boxData[0].ftm).toFixed(2)}`)
         .style("text-anchor", "left")
         .style("font-size", "20px")
         .style('fill', 'chartreuse')    
       
       svg.append("text")
         .attr("x", 260)
         .attr("y", 210)
         .text(`FTP: ${parseFloat(boxData[0].ft_pct).toFixed(2)}`)
         .style("text-anchor", "left")
         .style("font-size", "20px")
         .style('fill', 'chartreuse')
       
       svg.append("text")
         .attr("x", 260)
         .attr("y", 230)
         .text(`TPA: ${parseFloat(boxData[0].fg3m).toFixed(2)}`)
         .style("text-anchor", "left")
         .style("font-size", "20px")
         .style('fill', 'chartreuse')

       svg.append("text")
         .attr("x", 260)
         .attr("y", 250)
         .text(`TPM: ${parseFloat(boxData[0].fg3a).toFixed(2)}`)
         .style("text-anchor", "left")
         .style("font-size", "20px")
         .style('fill', 'chartreuse')

       svg.append("text")
         .attr("x", 260)
         .attr("y", 270)
         .text(`TPP: ${parseFloat(boxData[0].fg3_pct).toFixed(2)}`)
         .style("text-anchor", "left")
         .style("font-size", "20px")
         .style('fill', 'chartreuse')
       
       svg.append("text")
         .attr("x", 260)
         .attr("y", 290)
         .text(`REB: ${parseFloat(boxData[0].reb).toFixed(2)}`)
         .style("text-anchor", "left")
         .style("font-size", "20px")
         .style('fill', 'chartreuse')
       
       svg.append("text")
         .attr("x", 260)
         .attr("y", 310)
         .text(`AST: ${parseFloat(boxData[0].ast).toFixed(2)}`)
         .style("text-anchor", "left")
         .style("font-size", "20px")
         .style('fill', 'chartreuse')
       
       svg.append("text")
         .attr("x", 260)
         .attr("y", 330)
         .text(`STL: ${parseFloat(boxData[0].stl).toFixed(2)}`)
         .style("text-anchor", "left")
         .style("font-size", "20px")
         .style('fill', 'chartreuse')
       
       svg.append("text")
         .attr("x", 260)
         .attr("y", 350)
         .text(`TO: ${parseFloat(boxData[0].to).toFixed(2)}`)
         .style("text-anchor", "left")
         .style("font-size", "20px")
         .style('fill', 'chartreuse')
       
       svg.append("text")
         .attr("x", 260)
         .attr("y", 370)
         .text(`BLK: ${parseFloat(boxData[0].blk).toFixed(2)}`)
         .style("text-anchor", "left")
         .style("font-size", "20px")
         .style('fill', 'chartreuse')
       
       svg.append("text")
         .attr("x", 260)
         .attr("y", 390)
         .text(`P-M: ${parseFloat(boxData[0]['+/-']).toFixed(2)}`)
         .style("text-anchor", "left")
         .style("font-size", "20px")
         .style('fill', 'chartreuse')
       
       svg.append("text")
         .attr("x", 0)
         .attr("y", -65)
         .text(`${chartTitle}`)
         .style("text-anchor", "middle")
         .style("font-size", "40px")
         .style('fill', 'chartreuse')

       svg.append("line")
         .attr("x1", 60)
         .attr("x2", 60)
         .attr("y1", -52.5)
         .attr("y2", 137.5)
         .attr("stroke", "white")
         .attr("stroke-width", "2")

       svg.append("line")
         .attr("x1", -250)
         .attr("x2", 250)
         .attr("y1", 418)
         .attr("y2", 418)
         .attr("stroke", "white")
         .attr("stroke-width", "2")

       svg.append("line")
         .attr("x1", -60)
         .attr("x2", -60)
         .attr("y1", -52.5)
         .attr("y2", 137.5)
         .attr("stroke", "white")
         .attr("stroke-width", "2")

       svg.append("line")
         .attr("x1", -80)
         .attr("x2", 80)
         .attr("y1", 137.5)
         .attr("y2", 137.5)
         .attr("stroke", "white")
         .attr("stroke-width", "2")

       svg.append("line")
         .attr("x1", -80)
         .attr("x2", -80)
         .attr("y1", -52.5)
         .attr("y2", 137.5)
         .attr("stroke", "white")
         .attr("stroke-width", "2")

       svg.append("line")
         .attr("x1", 80)
         .attr("x2", 80)
         .attr("y1", -52.5)
         .attr("y2", 137.5)
         .attr("stroke", "white")
         .attr("stroke-width", "2")

       svg.append("line")
         .attr("x1", -219)
         .attr("x2", -221)
         .attr("y1", -52.5)
         .attr("y2", 87.5)
         .attr("stroke", "white")
         .attr("stroke-width", "2")

       svg.append("line")
         .attr("x1", 219)
         .attr("x2", 221)
         .attr("y1", -52.5)
         .attr("y2", 87.5)
         .attr("stroke", "white")
         .attr("stroke-width", "2")
       
       svg.append("line")
         .attr("x1", -30)
         .attr("x2", 30)
         .attr("y1", -8.25)
         .attr("y2", -8.25)
         .attr("stroke", "orange")
         .attr("stroke-width", "5")

       svg.append("line")
         .attr("x1", 80)
         .attr("x2", 87)
         .attr("y1", 100)
         .attr("y2", 100)
         .attr("stroke", "white")
         .attr("stroke-width", "4")
       
       svg.append("line")
         .attr("x1", -80)
         .attr("x2", -87)
         .attr("y1", 100)
         .attr("y2", 100)
         .attr("stroke", "white")
         .attr("stroke-width", "4")

       svg.append("line")
         .attr("x1", 80)
         .attr("x2", 87)
         .attr("y1", 70)
         .attr("y2", 70)
         .attr("stroke", "white")
         .attr("stroke-width", "4")
       
       svg.append("line")
         .attr("x1", -80)
         .attr("x2", -87)
         .attr("y1", 70)
         .attr("y2", 70)
         .attr("stroke", "white")
         .attr("stroke-width", "4")

       svg.append("line")
         .attr("x1", 80)
         .attr("x2", 87)
         .attr("y1", 40)
         .attr("y2", 40)
         .attr("stroke", "white")
         .attr("stroke-width", "4")
       
       svg.append("line")
         .attr("x1", -80)
         .attr("x2", -87)
         .attr("y1", 40)
         .attr("y2", 40)
         .attr("stroke", "white")
         .attr("stroke-width", "4")


       svg.append("line")
         .attr("x1", 80)
         .attr("x2", 87)
         .attr("y1", 30)
         .attr("y2", 30)
         .attr("stroke", "white")
         .attr("stroke-width", "4")
       
       svg.append("line")
         .attr("x1", -80)
         .attr("x2", -87)
         .attr("y1", 30)
         .attr("y2", 30)
         .attr("stroke", "white")
         .attr("stroke-width", "4")

       svg.append("line")
         .attr("x1", -250)
         .attr("x2", -250)
         .attr("y1", -52)
         .attr("y2", 418)
         .attr("stroke", "chartreuse")
         .attr("stroke-width", "2")

       // Y Axis
       const y = d3.scaleLinear()
         .domain([-52.5, 418])
         .range([ -52.5, 418]);

       svg.append("g")
         .attr("transform", "translate(250, 0)")
         .call(d3.axisLeft(y));

       // Dots
       d3.select(`#${myPlot}`).selectAll("circle").remove()

       svg.append("circle")
         .attr("cx", 0)
         .attr("cy", 0)
         .attr("r", 7.5)
         .attr("stroke", "orange")
         .style("stroke-width", 2)
         .style("fill", "none");

       svg.append("circle")
         .attr("cx", 0)
         .attr("cy", 137.5)
         .attr("r", 60)
         .style("opacity", .2)
         .attr("stroke", "white")
         .style("fill", "#33FFEC");
       
       svg.append("circle")
         .attr("cx", 0)
         .attr("cy", 418)
         .attr("r", 60)
         .style("opacity", .1)
         .attr("stroke", "white")
         .style("fill", "#33FFEC");

       svg.append("circle")
         .attr("cx", 0)
         .attr("cy", 418)
         .attr("r", 20)
         .attr("stroke", "white")
         .style("fill", "none");

       
       const arcGenerator2 = d3.arc()
         .outerRadius(61)
         .innerRadius(59)
         .startAngle(0)
         .endAngle(2*Math.PI);
       
       const halfCourtCircle = svg.append("path")
         .attr("transform", "translate(0, 418)")
         .attr("fill","white")
         .attr("d", arcGenerator2());

       const freeThrowCircle = svg.append("path")
         .attr("transform", "translate(0, 137.5)")
         .attr("fill","white")
         .attr("d", arcGenerator2());
       
       /*svg.append("circle")
         .attr("cx", 0)
         .attr("cy", 0)
         .attr("r", 235)
         .attr("stroke", "white")
         .style("fill", "none");
       *//*
const arcGenerator = d3.arc()
  .outerRadius(238.5)
  .innerRadius(236.5)
  .startAngle(Math.PI / 2 + 0.37855)
  .endAngle(Math.PI*3/2 - 0.37855);
 
const threeLine = svg.append("path")
  .attr("transform", "translate(0, 0)")
  .attr("fill","white")
  .attr("d", arcGenerator())
 
 
svg.append('g')
  .selectAll("dot")
  .data(dataMissedShots).enter()
  .append("circle")
  .attr("cx", function (d) { return d[0] } )
  .attr("cy", function (d) { return d[1] } )
  .attr("r", 2)
  .style("opacity", .7)
  .style("fill", "seagreen");

svg.append('g')
  .selectAll("dot")
  .data(dataMadeShots).enter()
  .append("circle")
  .attr("cx", function (d) { return d[0] } )
  .attr("cy", function (d) { return d[1] } )
  .style("opacity", .8)
  .attr("r", 2)
  .style("fill", "yellow");
}*/
