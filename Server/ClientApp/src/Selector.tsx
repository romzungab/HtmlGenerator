import * as React from 'react';
import Dropdown from "react-dropdown";
import { getDimensionAttributes } from './api';

export interface SelectorProps {
    name: string;
    placeholder: string;
    factTable: string;
    onChange?: (arg: string) => void;
    
}

interface SelectorState {
    options: string[];
    selectedValue: string;

}

class Selector extends React.Component<SelectorProps, SelectorState>{
    public readonly state: SelectorState = {options: [], selectedValue :""  }
    public async componentDidMount() {

        this.setState({
           options: await getDimensionAttributes(),
        });
    }

    public handleChange = (selectedOptions: string) => {
        this.setState({
            selectedValue: selectedOptions
        })
    }

    public render() {
        return (
            <div>
                <Dropdown options={this.state.options}
                    onChange={(e) => this.handleChange(e.value)}
                    placeholder={this.props.placeholder} />
            </div>
        )
    }
}


export default Selector;

