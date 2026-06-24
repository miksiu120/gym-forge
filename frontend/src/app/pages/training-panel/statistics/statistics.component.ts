import { DatePipe, DecimalPipe } from '@angular/common';
import { ChangeDetectorRef, Component, ElementRef, OnDestroy, OnInit, ViewChild } from '@angular/core';
import type { Chart as ChartInstance } from 'chart.js';
import { TrainingStatistics, TrainingWeightProgress } from '../../../interfaces/traning.interfaces';
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
  selectedExerciseIndex = 0;
  private chart?: ChartInstance<'line'>;
  private chartRenderId = 0;

  constructor(
    private readonly trainingPlans: TrainingPlanService,
    private readonly changeDetector: ChangeDetectorRef,
  ) {}

  get summary(): TrainingStatistics {
    return this.statistics!;
  }

  get selectedExercise(): TrainingWeightProgress | null {
    return this.statistics?.weightProgress[this.selectedExerciseIndex] ?? null;
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
    this.chartRenderId++;
    this.chart?.destroy();
  }

  selectExercise(index: number): void {
    if (!this.statistics?.weightProgress[index] || index === this.selectedExerciseIndex) return;

    this.selectedExerciseIndex = index;
    this.chartState = 'loading';
    void this.renderWeightChart();
  }

  moveExercise(direction: -1 | 1): void {
    const exerciseCount = this.statistics?.weightProgress.length ?? 0;
    if (exerciseCount < 2) return;

    const nextIndex = (this.selectedExerciseIndex + direction + exerciseCount) % exerciseCount;
    this.selectExercise(nextIndex);
  }

  maximumWeight(series: TrainingWeightProgress): number {
    return Math.max(...series.points.map(point => point.weight));
  }

  latestWeight(series: TrainingWeightProgress): number {
    return [...series.points]
      .sort((first, second) => new Date(second.completedAt).getTime() - new Date(first.completedAt).getTime())[0]
      ?.weight ?? 0;
  }

  private async renderWeightChart(): Promise<void> {
    const selectedExercise = this.selectedExercise;
    const renderId = ++this.chartRenderId;

    if (!selectedExercise || selectedExercise.points.length === 0) {
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

      if (renderId !== this.chartRenderId) return;

      const points = [...selectedExercise.points]
        .sort((first, second) =>
          new Date(first.completedAt).getTime() - new Date(second.completedAt).getTime());
      const labels = points.map(point => this.formatChartDate(point.completedAt));
      const weights = points.map(point => point.weight);
      const viewportWidth = this.weightChart.nativeElement.ownerDocument.defaultView?.innerWidth ?? 0;
      const chartFontSize = viewportWidth >= 1440 ? 16 : 13;

      this.chart?.destroy();
      this.chart = new Chart(this.weightChart.nativeElement, {
        type: 'line',
        data: {
          labels,
          datasets: [{
            label: selectedExercise.exerciseName,
            data: weights,
            borderColor: '#0f0f0f',
            backgroundColor: '#0f0f0f',
            borderWidth: 3,
            pointRadius: 5,
            pointHoverRadius: 7,
            pointHitRadius: 14,
            tension: 0.25,
          }],
        },
        options: {
          responsive: true,
          maintainAspectRatio: false,
          interaction: { mode: 'nearest', intersect: false },
          plugins: {
            legend: { display: false },
            tooltip: { callbacks: { label: (context) => `${context.dataset.label}: ${context.parsed.y} kg` } },
          },
          scales: {
            y: {
              beginAtZero: false,
              grace: '12%',
              ticks: { font: { size: chartFontSize } },
              title: { display: true, text: 'Weight (kg)', font: { size: chartFontSize + 1, weight: 500 } },
              grid: { color: '#00000012' },
            },
            x: {
              offset: points.length === 1,
              ticks: { font: { size: chartFontSize } },
              grid: { display: false },
            },
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
