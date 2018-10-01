import * as React from 'react';
import { getFactTables } from './api';
import './App.css';
import logo from './logo.svg';

interface State {
  factTables: string[];
}

class App extends React.Component<{}, State> {
  public readonly state: State = { factTables: [] };

  public async componentDidMount() {
    this.setState({
      factTables: await getFactTables(),
    })
  }

  public render() {
    return (
      <div className="App">
        <header className="App-header">
          <img src={logo} className="App-logo" alt="logo" />
          <h1 className="App-title">Welcome to React</h1>
        </header>
        <p className="App-intro">
          To get started, edit <code>src/App.tsx</code> and save to reload.
        </p>
          Fact Tables:
          {this.state.factTables.map((f, i) => <p key={i}>{f}</p>)}
      </div>
    );
  }
}

export default App;
