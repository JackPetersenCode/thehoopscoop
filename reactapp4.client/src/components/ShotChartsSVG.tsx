import React, { useState, useEffect } from 'react';
import { Shot } from '../interfaces/Shot';
import * as d3 from 'd3';


interface ShotChartsSVGProps {
    shotsData: Shot[];
    isGameChart: boolean;
}

const ShotChartSVG: React.FC<ShotChartsSVGProps> = ({ shotsData, isGameChart }) => {


    const [n, setN] = useState(
        parseInt(getComputedStyle(document.documentElement).getPropertyValue('--screen-size'))
    );

    const [myPlot] = useState(isGameChart ? "myPlot" : "myPlot2");
    //let [chartTitle, setCharTitle] = useState(isGameChart ? "SEASON SHOT CHART" : "GAME SHOT CHART");

    useEffect(() => {
        // Listen for changes in the CSS variable
        const resizeObserver = new ResizeObserver(() => {
            setN(parseInt(getComputedStyle(document.documentElement).getPropertyValue('--screen-size')));
        });
        resizeObserver.observe(document.documentElement);

        // Cleanup function
        return () => resizeObserver.disconnect();
    }, []);

    useEffect(() => {
        const getSVG = async () => {

            //const data = [];
            const dataMadeShots = [];
            const dataMissedShots = [];

            d3.select(`#${myPlot}`).selectAll("*").remove();


            for (let i = 0; i < shotsData.length; i++) {
                //data.push([totalShotsArray.resultSets[0].rowSet[i][17], totalShotsArray.resultSets[0].rowSet[i][18]]);
                if (shotsData[i].shot_made_flag === "1") {
                    dataMadeShots.push([shotsData[i].loc_x, shotsData[i].loc_y])
                } else {
                    dataMissedShots.push([shotsData[i].loc_x, shotsData[i].loc_y]);
                }
            }


            const xSize = 620 / n;
            const xPosHalf = 380 / n;
            const xMargin = 120 / n;
            const yMargin = 60 / n;
            const width = xSize - xMargin
            const halfPosWidth = xPosHalf - xMargin;
            const svg = d3.select(`#${myPlot}`)
                .append("svg")
                .append("g")
                .attr("transform", "translate(" + halfPosWidth + ", " + yMargin + ")")
                .style('color', 'white');

            //d3.select(`#${myPlot}`).selectAll("text").remove()
            // X Axis
            const x = d3.scaleLinear()
                .domain([-250 / n, 250 / n])
                .range([0, width])


            svg.append("g")
                .attr("transform", `translate(${-250 / n}, ${-52.5 / n})`)
                .call(d3.axisBottom(x).tickFormat(() => "")) // Remove tick labels
                .style('color', 'white');

            svg.append("line")
                .attr("x1", 60 / n)
                .attr("x2", 60 / n)
                .attr("y1", -52.5 / n)
                .attr("y2", 137.5 / n)
                .attr("stroke", "white")
                .attr("stroke-width", `${2 / n}`)

            svg.append("line")
                .attr("x1", -250 / n)
                .attr("x2", 250 / n)
                .attr("y1", 418 / n)
                .attr("y2", 418 / n)
                .attr("stroke", "white")
                .attr("stroke-width", `${2 / n}`)

            svg.append("line")
                .attr("x1", -60 / n)
                .attr("x2", -60 / n)
                .attr("y1", -52.5 / n)
                .attr("y2", 137.5 / n)
                .attr("stroke", "white")
                .attr("stroke-width", `${2 / n}`)

            svg.append("line")
                .attr("x1", -80 / n)
                .attr("x2", 80 / n)
                .attr("y1", 137.5 / n)
                .attr("y2", 137.5 / n)
                .attr("stroke", "white")
                .attr("stroke-width", `${2 / n}`)

            svg.append("line")
                .attr("x1", -80 / n)
                .attr("x2", -80 / n)
                .attr("y1", -52.5 / n)
                .attr("y2", 137.5 / n)
                .attr("stroke", "white")
                .attr("stroke-width", `${2 / n}`)

            svg.append("line")
                .attr("x1", 80 / n)
                .attr("x2", 80 / n)
                .attr("y1", -52.5 / n)
                .attr("y2", 137.5 / n)
                .attr("stroke", "white")
                .attr("stroke-width", `${2 / n}`)

            svg.append("line")
                .attr("x1", -219 / n)
                .attr("x2", -221 / n)
                .attr("y1", -52.5 / n)
                .attr("y2", 87.5 / n)
                .attr("stroke", "white")
                .attr("stroke-width", `${2 / n}`)

            svg.append("line")
                .attr("x1", 219 / n)
                .attr("x2", 221 / n)
                .attr("y1", -52.5 / n)
                .attr("y2", 87.5 / n)
                .attr("stroke", "white")
                .attr("stroke-width", `${2 / n}`)
            svg.append("line")
                .attr("x1", -30 / n)
                .attr("x2", 30 / n)
                .attr("y1", -8.25 / n)
                .attr("y2", -8.25 / n)
                .attr("stroke", "orange")
                .attr("stroke-width", `${5 / n}`)

            svg.append("line")
                .attr("x1", 80 / n)
                .attr("x2", 87 / n)
                .attr("y1", 100 / n)
                .attr("y2", 100 / n)
                .attr("stroke", "white")
                .attr("stroke-width", `${4 / n}`)
            svg.append("line")
                .attr("x1", -80 / n)
                .attr("x2", -87 / n)
                .attr("y1", 100 / n)
                .attr("y2", 100 / n)
                .attr("stroke", "white")
                .attr("stroke-width", `${4 / n}`)

            svg.append("line")
                .attr("x1", 80 / n)
                .attr("x2", 87 / n)
                .attr("y1", 70 / n)
                .attr("y2", 70 / n)
                .attr("stroke", "white")
                .attr("stroke-width", `${4 / n}`)
            svg.append("line")
                .attr("x1", -80 / n)
                .attr("x2", -87 / n)
                .attr("y1", 70 / n)
                .attr("y2", 70 / n)
                .attr("stroke", "white")
                .attr("stroke-width", `${4 / n}`)

            svg.append("line")
                .attr("x1", 80 / n)
                .attr("x2", 87 / n)
                .attr("y1", 40 / n)
                .attr("y2", 40 / n)
                .attr("stroke", "white")
                .attr("stroke-width", `${4 / n}`)
            svg.append("line")
                .attr("x1", -80 / n)
                .attr("x2", -87 / n)
                .attr("y1", 40 / n)
                .attr("y2", 40 / n)
                .attr("stroke", "white")
                .attr("stroke-width", `${4 / n}`)


            svg.append("line")
                .attr("x1", 80 / n)
                .attr("x2", 87 / n)
                .attr("y1", 30 / n)
                .attr("y2", 30 / n)
                .attr("stroke", "white")
                .attr("stroke-width", `${4 / n}`)
            svg.append("line")
                .attr("x1", -80 / n)
                .attr("x2", -87 / n)
                .attr("y1", 30 / n)
                .attr("y2", 30 / n)
                .attr("stroke", "white")
                .attr("stroke-width", `${4 / n}`)

            svg.append("line")
                .attr("x1", -250 / n)
                .attr("x2", -250 / n)
                .attr("y1", -52 / n)
                .attr("y2", 418 / n)
                .attr("stroke", "white")
            // Y Axis
            const y = d3.scaleLinear()
                .domain([-52.5 / n, 418 / n])
                .range([-52.5 / n, 418 / n]);

            svg.append("g")
                .attr("transform", `translate(${250 / n}, 0)`)
                .call(d3.axisLeft(y).tickFormat(() => ""));

            // Dots
            d3.select(`#${myPlot}`).selectAll("circle").remove()

            svg.append("circle")
                .attr("cx", 0)
                .attr("cy", 0)
                .attr("r", 7.5 / n)
                .attr("stroke", "orange")
                .style("stroke-width", 2 / n)
                .style("fill", "none");

            svg.append("circle")
                .attr("cx", 0)
                .attr("cy", 137.5 / n)
                .attr("r", 60 / n)
                .style("opacity", .2)
                .attr("stroke", "white")
                .style("fill", "rgb(65,65,65)");
            svg.append("circle")
                .attr("cx", 0)
                .attr("cy", 418 / n)
                .attr("r", 60 / n)
                .style("opacity", .2)
                .attr("stroke", "white")
                .style("fill", "rgb(65,65,65)");

            svg.append("circle")
                .attr("cx", 0)
                .attr("cy", 418 / n)
                .attr("r", 20 / n)
                .attr("stroke", "white")
                .style("fill", "none");

            const arcGenerator2 = d3.arc<d3.DefaultArcObject>()
                .outerRadius(61 / n)
                .innerRadius(59 / n)
                .startAngle(0)
                .endAngle(2 * Math.PI);

            const defaultArcData: d3.DefaultArcObject = {
                innerRadius: 0,
                outerRadius: 0,
                startAngle: 0,
                endAngle: 0
            };


            svg.append("path")
                .attr("transform", `translate(0, ${418 / n})`)
                .attr("fill", "white")
                .attr("d", arcGenerator2(defaultArcData));

            svg.append("path")
                .attr("transform", `translate(0, ${137.5 / n})`)
                .attr("fill", "white")
                .attr("d", arcGenerator2(defaultArcData));
            /*svg.append("circle")
              .attr("cx", 0)
              .attr("cy", 0)
              .attr("r", 235)
              .attr("stroke", "white")
              .style("fill", "none");
            */
            const arcGenerator = d3.arc()
                .outerRadius(238.5 / n)
                .innerRadius(236.5 / n)
                .startAngle(Math.PI / 2 + 0.37855)
                .endAngle(Math.PI * 3 / 2 - 0.37855);
            svg.append("path")
                .attr("transform", "translate(0, 0)")
                .attr("fill", "white")
                .attr("d", arcGenerator(defaultArcData))

            svg.append('g')
                .selectAll("dot")
                .data(dataMissedShots).enter()
                .append("circle")
                .attr("cx", function (d: string[]) { return (parseFloat(d[0]) / n).toString() })
                .attr("cy", function (d: string[]) { return (parseFloat(d[1]) / n).toString() })
                .attr("r", 2 / n)
                .style("opacity", .7)
                .style("fill", "rgba(64, 154, 244, 0.5)");

            svg.append('g')
                .selectAll("dot")
                .data(dataMadeShots).enter()
                .append("circle")
                .attr("cx", function (d: string[]) { return (parseFloat(d[0]) / n).toString() })
                .attr("cy", function (d: string[]) { return (parseFloat(d[1]) / n).toString() })
                .style("opacity", .8)
                .attr("r", 2 / n)
                .style("fill", "rgb(191, 255, 0)");
        }
        if (shotsData) {
            console.log(shotsData)
            //console.log(boxData)
            getSVG();
        }
    }, [shotsData, n])

    return (
        <div>
            
            <svg id={myPlot}
                style={{
                    height: 470 / n,
                    width: 520 / n,
                    margin: "auto",
                    backgroundColor: "black",
                }}
            >
            </svg>

        </div>
    );
}

export default ShotChartSVG;

