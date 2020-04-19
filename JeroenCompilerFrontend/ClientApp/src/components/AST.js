import React, { Component } from 'react';
import AceEditor from "react-ace";
import "ace-builds/src-noconflict/mode-csharp";
import "ace-builds/src-noconflict/theme-github";
import LoadingOverlay from 'react-loading-overlay';


export class AST extends Component {
    static displayName = AST.name;

    constructor(props) {
        super(props);
        this.state = {
            input: '{\r\n\tint bar;\r\n\r\n\tbar = 0;\r\n\r\n\tif(bar < 10) {\r\n\t\tbar = 1;\r\n\t}\r\n\telse if (bar < 20) {\r\n\t\tbar = 2;\r\n\t}\r\n\telse {\r\n\t\tbar = 3;\r\n\t}\r\n\r\n\tbar = 4;\r\n}',
            il: '',
            loading: true
        };

        this.onChange = this.onChange.bind(this);
        this.compile = this.compile.bind(this);
    }

    onChange(newValue) {
        this.setState({ input: newValue });
    }

    componentDidMount() {
        this.compile();
    }

    compile() {
        this.setState({ loading: true });
        this.refs.graphContainer1.innerHTML = '';

        return fetch("/api/compiler/ast", {
            body: JSON.stringify({
                input: this.state.input
            }),
            headers: {
                'Content-Type': 'application/json'
            },
            method: "post"
        })
        .then(res => res.json())
        .then(res => {
            this.setState({ ast: res.ast });

            let viz = new window.Viz({ workerURL: 'full.render.js' });

            return viz.renderSVGElement(res.ast)
            .then(element => {
                this.refs.graphContainer1.append(element);
            })
            .catch(error => {
                viz = new window.Viz({ workerURL: 'full.render.js' });
                console.error(error);
            });
        })
        .then(() => {
          this.setState({ loading: false });
        });
    }

    render() {
        return (
            <div className="repl">
                <h1>Abstract Syntax Tree</h1>

                <div className="row">
                    <div className="left">
                        <p>Source text</p>

                        <button onClick={this.compile}>Compile</button>
                        <AceEditor
                            mode="csharp"
                            theme="github"
                            onChange={this.onChange}
                            name="left"
                            value={this.state.input}
                            editorProps={{ $blockScrolling: true }}
                        />
                    </div>

                    <div className="right">
                        <p>AST</p>
                        <LoadingOverlay
                            active={this.state.loading}
                            spinner
                            text='Loading.....'
                            >
                              { this.state.loading && <p>Loading</p>}
                              <div ref="graphContainer1" className="graphContainer1">
                              </div>
                        </LoadingOverlay>
                    </div>
                </div>
            </div>
        );
    }
}
