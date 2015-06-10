using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Yanoshi.CalcHLACGUI.Common
{


    /*
     * http://www.makcraft.com/blog/meditation/2013/12/29/commands-that-take-arguments/
     * 
    Copyright © 2012, 配島誠
    All rights reserved.
    ソースコード形式かバイナリ形式か、変更するかしないかを問わず、以下の条件
    を満たす場合に限り、再頒布および使用が許可されます。
    • ソースコードを再頒布する場合、上記の著作権表示、本条件一覧、および下
    記免責条項を含めること。
    • バイナリ形式で再頒布する場合、頒布物に付属のドキュメント等の資料に、
    上記の著作権表示、本条件一覧、および下記免責条項を含めること。
    • 書面による特別の許可なしに、本ソフトウェアから派生した製品の宣伝また
    は販売促進に、'MAKCRAFT.COM'の名前またはコントリビューターの名前を使
    用してはならない。
    本ソフトウェアは、著作権者およびコントリビューターによって「現状のまま」
    提供されており、明示黙示を問わず、商業的な使用可能性、および特定の目的に対
    する適合性に関する暗黙の保証も含め、またそれに限定されない、いかなる保証も
    ありません。著作権者もコントリビューターも、事由のいかんを問わず、 損害発生
    の原因いかんを問わず、かつ責任の根拠が契約であるか厳格責任であるか（過失そ
    の他の）不法行為であるかを問わず、仮にそのような損害が発生する可能性を知ら
    されていたとしても、本ソフトウェアの使用によって発生した（代替品または代用
    サービスの調達、使用の喪失、データの喪失、利益の喪失、業務の中断も含め、ま
    たそれに限定されない）直接損害、間接損害、偶発的な損害、特別損害、懲罰的損
    害、または結果損害について、一切責任を負わないものとします。 
     * http://www.makcraft.com/storehouse/LICENSE.pdf
     */
    /// <summary>
    /// デリゲートを呼び出すことによって、コマンドを他のオブジェクトに中継する。CanExecute メソッドの既定値は 'true'。
    /// </summary>
    public class RelayCommand : ICommand
    {

        #region fields
        private readonly Action<object> _executeOld;
        private readonly Action _execute;
        private readonly Predicate<object> _canExecute;
        #endregion // Fields


        #region Constructor
        /// <summary>
        /// 実行可否判定のないコマンドを作成
        /// </summary>
        /// <param name="execute"></param>
        [Obsolete("RelayCommand(Action execute) または RelayCommand(Action<T> execute) を使用してください。")]
        public RelayCommand(Action<object> execute) : this(execute, null) { }


        /// <summary>
        /// 実行可否判定のないコマンドを作成
        /// </summary>
        /// <param name="execute"></param>
        public RelayCommand(Action execute) : this(execute, null) { }


        /// <summary>
        /// コマンドを作成
        /// </summary>
        /// <param name="execute"></param>
        /// <param name="canExecute"></param>
        [Obsolete("RelayCommand(Action execute, Predicate<object> canExecute) または RelayCommand(Action<T> execute, Predicate<object> canExecute) を使用してください。")]
        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("param: execute");
            _executeOld = execute;
            _canExecute = canExecute;
        }


        /// <summary>
        /// コマンドを作成
        /// </summary>
        /// <param name="execute"></param>
        /// <param name="canExecute"></param>
        public RelayCommand(Action execute, Predicate<object> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("param: execute");
            _execute = execute;
            _canExecute = canExecute;
        }
        #endregion // Constructor
 

        #region ICommand Members
        /// <summary>
        /// 現在の状態でこのコマンドを実行できるかどうかを判断します。
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(object parameter)
        {
            return _canExecute == null ? true : _canExecute(parameter);
        }


        /// <summary>
        /// コマンドを実行するかどうかに影響するような変更があった場合に発生するイベントです。
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (_canExecute != null)
                    CommandManager.RequerySuggested += value;
            }
            remove
            {
                if (_canExecute != null)
                    CommandManager.RequerySuggested -= value;
            }
        }


        /// <summary>
        /// コマンドの起動時に呼び出されるメソッドです。
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            if (_execute != null)
                _execute();
            else
                _executeOld(parameter);
        }
        #endregion // ICommand Members

    }


    /// <summary>
    /// デリゲートを呼び出すことによって、コマンドを他のオブジェクトに中継する。CanExecute メソッドの既定値は 'true'。
    /// </summary>
    public class RelayCommand<T> : ICommand
    {
        #region fields
        private readonly Action<T> _execute;
        private readonly Predicate<object> _canExecute;
        #endregion // Fields


        #region Constructor
        /// <summary>
        /// 実行可否判定のないコマンドを作成
        /// </summary>
        /// <param name="execute"></param>
        public RelayCommand(Action<T> execute)
            : this(execute, null)
        {

        }


        /// <summary>
        /// コマンドを作成
        /// </summary>
        /// <param name="execute"></param>
        /// <param name="canExecute"></param>
        public RelayCommand(Action<T> execute, Predicate<object> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("param: execute");

            _execute = execute;
            _canExecute = canExecute;
        }
        #endregion // Constructor


        #region ICommand Members
        /// <summary>
        /// 現在の状態でこのコマンドを実行できるかどうかを判断します。
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(object parameter)
        {
            return _canExecute == null ? true : _canExecute(parameter);
        }


        /// <summary>
        /// コマンドを実行するかどうかに影響するような変更があった場合に発生するイベントです。
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (_canExecute != null)
                    CommandManager.RequerySuggested += value;
            }
            remove
            {
                if (_canExecute != null)
                    CommandManager.RequerySuggested -= value;
            }
        }


        /// <summary>
        /// コマンドの起動時に呼び出されるメソッドです。
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            _execute((T)parameter);
        }

        #endregion // ICommand Members
    }

}
