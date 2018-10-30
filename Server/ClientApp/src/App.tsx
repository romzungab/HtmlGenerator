import * as React from 'react';
import Dropdown from 'react-dropdown';
import 'react-dropdown/style.css';
import Select from 'react-select';
import { ValueType } from 'react-select/lib/types';
import { BeatLoader } from 'react-spinners';
import { Dimension, FactTable, getFactTables } from './api';
import './App.css';
import logo from './logo.svg';

interface State {
  factTables: { [key: string]: FactTable };
  factTable?: string; // the selected factTable
  columns: string[];
  loading: boolean;
  dimensionAttributes: DimensionAttribute[];
  hide: boolean;
}

interface DimensionAttribute {
  name: string;
  dimensionName: string;
  displayName: string;
}

// interface Column extends DimensionAttribute {
//   pivot: boolean;
// }

class App extends React.Component<{}, State> {
  public readonly state: State = {
    factTables: {},
    columns: [],
    loading: true,
    dimensionAttributes:[],
    hide: false
  }

  public async componentDidMount() {
    const factTables = await getFactTables();
    const factTableMap = {};
    for (const ft of factTables) {
      factTableMap[ft.name] = ft;
    }

    this.setState({
      factTables: factTableMap,
      loading: false
    });
  }

  public render() {
    const { factTables, loading, columns } = this.state;

    let data;
    const c = "";
    if (loading) {
      data = <div className='sweet-loading'>
        <BeatLoader
          sizeUnit={"px"}
          size={15}
          color={'#123abc'}
          margin={"2px"}
          loading={loading}
        />
      </div>
    } else {
      data = <div>
        <p>You selected fact table: {this.state.factTable}</p>
        Columns:
             {this.state.columns.map((f, i) => <p key={i}>{f}</p>)}

      </div>
    }

    return (

      <div className="App">
        <header className="App-header">
          <img src={logo} className="App-logo" alt="logo" />
          <h1 className="App-title">Welcome to React</h1>
        </header>
        <div className="App-sidenav">
          <div className="factTable" >
            <Select options={Object.keys(factTables).map(ft => ({ label: ft, value: ft }))}
              placeholder="Select a fact table"
              onChange={e => this.onSelectFactTable(e)} />

            <p>Columns</p>
              {(columns.length > 0) &&
                  columns.map(col => this.renderAttributeSelector(col))}
              { this.renderAttributeSelector() }
              
          </div>
        </div>
        <div id="main">
          {data} {c}
        </div>
      </div>
    );
  }

  private renderAttributeSelector(col?: string): JSX.Element {
    
    return <div className="dropdownRow">
      <Dropdown options={this.getAttributes().map( da => ({label:da.displayName, value:da.displayName}))} 
              onChange={(e) => { this.onChangeAttr(e.value, col); }} placeholder="Select columns" value={col} hide={this.state.hide}/>
      <label className="labelPivot">
        <input type="checkbox" />
            <button onClick={(e) => {alert("closing"); this.setState({ hide: true })}}>x</button>
        pivot
      </label>
    </div>;
  }

  private getAttributes() {
    const { factTable, factTables, columns } = this.state;
    if (!factTable) { return [] }
    return factTables[factTable].dimensions
      .map(d => d.attributes.map(a => this.getAttribute(d, a, d.attributes.length === 1)))
      .reduce((acc, attrs) => acc.concat(attrs), [])
      .filter(a => !columns.includes(a.displayName));
  }

  private getAttribute(d: Dimension, attrName: string, single: boolean) : DimensionAttribute {
    return {
      name: attrName,
      dimensionName: d.name,
      displayName: single ? attrName : `${d.name}.${attrName}`,
    }
  }

  private onChangeAttr (value: string, currentAttr?: string) {
    let { columns } = this.state;

    if (currentAttr === undefined) {
      this.setState({
        columns: [...columns, value]
      })
    } else {
      columns = [ ...columns ];
      const ind = columns.indexOf(currentAttr);
      columns.splice(ind, 1, value);
      this.setState({ columns });
    }
  }

  private onSelectFactTable(e: ValueType<{ value: string }>) {
    if (!e) { return }
    this.setState({
      factTable: Array.isArray(e) ? e[0].value : e.value,
      columns: [],
    }); 
  }

}




export default App;
