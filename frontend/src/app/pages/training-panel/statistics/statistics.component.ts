import { DatePipe, DecimalPipe } from '@angular/common';
import { ChangeDetectorRef, Component, ElementRef, OnDestroy, OnInit, ViewChild } from '@angular/core';
import type { Chart as ChartInstance } from 'chart.js';
import { TrainingStatistics } from '../../../interfaces/traning.interfaces';
import { TrainingPlanService } from '../../../services/training-plan-service/training-plan.service';

type ChartState = 'loading' | 'ready' | 'empty' | 'error';

@Component({
  selector: 'component-statistics',
  imports: [DatePipe, DecimalPipe],
  templateUrl: './statistics.component.html',
  styleUrl: './statistics.component.scss',
})
export class StatisticsComponent implements OnInit, OnDestroy {
  @ViewChild('weightChart') weightChart?: ElementRef<HTMLCanvasElement>;

  statistics: TrainingStatistics | null = null;
  loading = true;
  errorMessage = '';
  chartState: ChartState = 'loading';
  private chart?: ChartInstance<'line'>;

  constructor(
    private readonly trainingPlans: TrainingPlanService,
    private readonly changeDetector: ChangeDetectorRef,
  ) {}

  get summary(): TrainingStatistics {
    return this.statistics!;
  }

  ngOnInit(): void {
    this.trainingPlans.getStatistics().subscribe({
      next: (statistics) => {
        this.statistics = statistics;
        this.loading = false;
        this.chartState = statistics.weightProgress.length > 0 ? 'loading' : 'empty';
        this.changeDetector.detectChanges();
        void this.renderWeightChart();
      },
      error: () => {
        this.loading = false;
        this.errorMessage = 'Statistics could not be loaded right now. Please try again later.';
      },
    });
  }

  ngOnDestroy(): void {
    this.chart?.destroy();
  }

  private async renderWeightChart(): Promise<void> {
    if (!this.statistics || this.statistics.weightProgress.length === 0) {
      this.chartState = 'empty';
      return;
    }

    if (!this.weightChart) {
      this.chartState = 'error';
      return;
    }

    try {
      const {
        CategoryScale,
        Chart,
        Legend,
        LineController,
        LineElement,
        LinearScale,
        PointElement,
        Tooltip,
      } = await import('chart.js');
      Chart.register(CategoryScale, LinearScale, PointElement, LineElement, LineController, Tooltip, Legend);

      const dates = [...new Set(this.statistics.weightProgress
        .flatMap(series => series.points.map(point => point.completedAt)))]
        .sort((first, second) => new Date(first).getTime() - new Date(second).getTime());

      if (dates.length === 0) {
        this.chartState = 'empty';
        return;
      }

      const labels = dates.map((date) => this.formatChartDate(date));
      const colors = ['#111111', '#4a4a4a', '#777777', '#9d9d9d', '#c2c2c2'];
      const viewportWidth = this.weightChart.nativeElement.ownerDocument.defaultView?.innerWidth ?? 0;
      const chartFontSize = viewportWidth >= 1440 ? 14 : 12;

      this.chart?.destroy();
      this.chart = new Chart(this.weightChart.nativeElement, {
        type: 'line',
        data: {
          labels,
          datasets: this.statistics.weightProgress.map((series, index) => ({
            label: series.exerciseName,
            data: dates.map((date) => series.points.find(point => point.completedAt === date)?.weight ?? null),
            borderColor: colors[index % colors.length],
            backgroundColor: colors[index % colors.length],
            borderWidth: 2,
            pointRadius: 3,
            pointHoverRadius: 5,
            tension: 0.3,
            spanGaps: true,
          })),
        },
        options: {
          responsive: true,
          maintainAspectRatio: false,
          interaction: { mode: 'index', intersect: false },
          plugins: {
            legend: {
              position: 'bottom',
              labels: { usePointStyle: true, boxWidth: 8, padding: 16, font: { size: chartFontSize } },
            },
            tooltip: { callbacks: { label: (context) => `${context.dataset.label}: ${context.parsed.y} kg` } },
          },
          scales: {
            y: {
              beginAtZero: true,
              ticks: { font: { size: chartFontSize } },
              title: { display: true, text: 'Weight (kg)', font: { size: chartFontSize } },
              grid: { color: '#00000012' },
            },
            x: { ticks: { font: { size: chartFontSize } }, grid: { display: false } },
          },
        },
      });
      this.chartState = 'ready';
      this.changeDetector.detectChanges();
    } catch {
      this.chartState = 'error';
      this.changeDetector.detectChanges();
    }
  }

  private formatChartDate(date: string): string {
    return new Intl.DateTimeFormat('en-GB', { day: 'numeric', month: 'short' }).format(new Date(date));
  }
}
