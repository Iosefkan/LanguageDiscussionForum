<script lang="ts">
    import { Line } from 'svelte-chartjs';
    export let dateLoginCount : any = "";

    let data : any = {
    labels: ['January', 'February', 'March', 'April', 'May', 'June', 'July'],
    datasets: [
      {
        label: 'Visit count per day',
        fill: true,
        lineTension: 0.3,
        backgroundColor: 'rgba(225, 204,230, .3)',
        borderColor: 'rgb(205, 130, 158)',
        borderCapStyle: 'butt',
        borderDash: [],
        borderDashOffset: 0.0,
        borderJoinStyle: 'miter',
        pointBorderColor: 'rgb(205, 130,1 58)',
        pointBackgroundColor: 'rgb(255, 255, 255)',
        pointBorderWidth: 10,
        pointHoverRadius: 5,
        pointHoverBackgroundColor: 'rgb(0, 0, 0)',
        pointHoverBorderColor: 'rgba(220, 220, 220,1)',
        pointHoverBorderWidth: 2,
        pointRadius: 1,
        pointHitRadius: 10,
        data: [65, 59, 80, 81, 56, 55, 40],
      }
    ],
  };

    import {
      Chart as ChartJS,
      Title,
      Tooltip,
      Legend,
      LineElement,
      LinearScale,
      PointElement,
      CategoryScale,
    } from 'chart.js';
  
    ChartJS.register(
      Title,
      Tooltip,
      Legend,
      LineElement,
      LinearScale,
      PointElement,
      CategoryScale
    );
    function splitData() : boolean{
      let dateLabels : Array<string> = [];
      let loginCounts : Array<number> = [];
      for (let dateLogCount of dateLoginCount){
        dateLabels.push(dateLogCount.date);
        loginCounts.push(dateLogCount.loginCount);
      }
      data.labels = dateLabels;
      data.datasets[0].data = loginCounts;
      return true;
    }
  </script>
  
  {#if (dateLoginCount)}
    {#if (splitData())}
      <Line {data} options={{ responsive: true }} />
    {/if}
  {:else}
    <Line {data} options={{ responsive: true }} />
  {/if}
  