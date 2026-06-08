import { useWeatherForecastHook } from './hooks/useWetherForecastHook';
import './App.css';

function App() {
	const { forecasts, error, loading } = useWeatherForecastHook();

	return (
		<div className='container'>
			<h1>SubsAPP - Połączenie z Backendem .NET</h1>

			<div className='card'>
				{loading && <p>Ładowanie danych...</p>}
				{error && <p style={{ color: 'red' }}>Błąd: {error.message}</p>}
				{!error && !loading && (
					<p>
						Poniżej znajduje się tabela z danymi pogodowymi pobranymi z
						backendowego API .NET. Każdy wiersz reprezentuje prognozę pogody na
						dany dzień, zawierając datę, temperaturę w stopniach Celsjusza oraz
						krótkie podsumowanie warunków pogodowych.
					</p>
				)}
				<table
					border={1}
					style={{ width: '100%', marginTop: '10px', textAlign: 'left' }}
				>
					<thead>
						<tr>
							<th>Data</th>
							<th>Temp (°C)</th>
							<th>Podsumowanie</th>
						</tr>
					</thead>
					<tbody>
						{forecasts.map((f, index) => (
							<tr key={index}>
								<td>{new Date(f.date).toLocaleDateString()}</td>
								<td>{f.temperatureC}°C</td>
								<td>{f.summary}</td>
							</tr>
						))}
					</tbody>
				</table>
			</div>
		</div>
	);
}

export default App;
